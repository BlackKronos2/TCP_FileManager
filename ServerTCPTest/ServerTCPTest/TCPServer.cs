using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection;
using System.Data;
using System.Threading;
using System.Net.Http.Headers;

namespace ServerTCPTest
{
    public class TCPServer 
    {
        const string ServerDirectoryPath = "ServerDirectory";
        public static readonly string[] CommadsList = { "GetContent", "GetFile" };

        private List<Timer> _activeTimers = new List<Timer>(0);
        const int timerUser = 20;

        private string GetClientCommand(Socket tcpClient)
        {
            byte[] bytes = new byte[1024];

            Console.WriteLine("Обработка команды...");

            int bytesRec = tcpClient.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);


            return data;
        }

        private byte[] GetDirectoryСontentList(string directoryPath)
        {
            string Content = "";
            string FullDirectoryPath = (directoryPath == "") ? (ServerDirectoryPath):(directoryPath);

            Content += FullDirectoryPath + "\n";

            string[] DirectoryList = Directory.GetDirectories(FullDirectoryPath);
            foreach (string directory in DirectoryList)
                Content += directory + "\n";

            string[] FilesList = Directory.GetFiles(FullDirectoryPath);
            foreach (string file in FilesList)
                Content += file + "\n";

            return Encoding.UTF8.GetBytes(Content);
        }

        private byte[] GetFile(string FilePath)
        {
            string FullDirectoryPath = FilePath;
            byte[] fileBytes = File.ReadAllBytes(FullDirectoryPath);

            return fileBytes;
        }

        private void ClientShutDown(Socket tcpClient)
        {
            tcpClient.Shutdown(SocketShutdown.Both);
            tcpClient.Close();
            Console.WriteLine("Сеанс завершен по истечению времени");
        }

        private int RunActionAfter(Action action, int period)
        {
            Timer timer = new Timer(_ => action(), null, period, -1);
            _activeTimers.Add(timer);
            int timerId = _activeTimers.Count - 1;
            return timerId;
        }

        private async Task ProcessClientAsync(Socket tcpClient)
        {
            Console.WriteLine("Подключение успешно");

            int Id = RunActionAfter(() => ClientShutDown(tcpClient), timerUser * 1000);

            byte[] bytes = new byte[1024];

            while (true)
            {
                Console.WriteLine($"Ждем команд");
                string clientCommand = GetClientCommand(tcpClient);
                string path = null;

                _activeTimers[Id].Dispose();
                _activeTimers.RemoveAt(Id);

                Id = RunActionAfter(() => ClientShutDown(tcpClient), 10000);

                Console.WriteLine($"Команда: {clientCommand}");

                switch (clientCommand)
                {
                    case var s when clientCommand.Contains(CommadsList[0]):
                        path = clientCommand.Replace(CommadsList[0], "");
                        Console.WriteLine("Ищем содержимое");
                        bytes = GetDirectoryСontentList(path);
                        break;
                    case var s when clientCommand.Contains(CommadsList[1]):
                        path = clientCommand.Replace(CommadsList[1], "");
                        Console.WriteLine("Ищем файл");
                        bytes = GetFile(path);
                        break;
                }


                // Отправляем ответ клиенту
                tcpClient.Send(bytes);
            }
            tcpClient.Shutdown(SocketShutdown.Both);
            tcpClient.Close();
            Console.WriteLine("Сеанс завершен");
        }

        public void ServerStart()
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            //IPAddress ipAddr = ipHost.AddressList[0];
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 58000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket socket = null;
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    socket = handler;
                    //
                    Task.Run(async () => await ProcessClientAsync(handler));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                byte[] buffer1 = new byte[1024];
                buffer1 = Encoding.UTF8.GetBytes("error");
                socket.Send(buffer1);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}

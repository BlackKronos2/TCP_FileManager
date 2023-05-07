using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace ClientTCP
{
    public sealed class CommandMaster
    {
        private static CommandMaster _instance = null;
        private bool _list_received = false;
        private string _userPath;

        private CommandMaster()
        { }

        public static CommandMaster GetInstance()
        {
            if (_instance == null)
                _instance = new CommandMaster();
            return _instance;
        }

        public string Start(Button buttonGetListFiles)
        {
            Socket client;
            try
            {
                client = NetTCP.Connection();
                return "Сокет соединен с {0} " + client.RemoteEndPoint.ToString();
            }
            catch
            {
                buttonGetListFiles.IsEnabled = false;
                return "-Ошибка подключения к серверу-";
            }
        }

        public List<string> RequestDirectory(string elementPath)
        {
            if (!NetTCP.Connection().Connected)
                throw new Exception("Сервер отключился");

            if (!_list_received)
                NetTCP.SendMessage("GetContent"); //GetContent + ""
            else
                NetTCP.SendMessage($"GetContent{elementPath}");

            string answer = Encoding.UTF8.GetString(NetTCP.GetMessage()); //Путь\nДиректория1\nДиректория2
            if (answer == "")
            {
                throw new Exception("Ошибка при получении ответа от сервера");
            }

            string[] Files = answer.Split('\n');
            for (int i = 1; i < Files.Length - 1; i++)
                Files[i] = Files[i].Replace(Files[0] + "\\", "");

            List<string> ListFiles = new List<string>(0);
            for (int i = 0; i < Files.Length-1; i++)
                ListFiles.Add(Files[i]);

            _list_received = true;
            _userPath = ListFiles[0];
            return ListFiles;
        }

        public void RequestFile(string elementPath)
        {
            if (!NetTCP.Connection().Connected)
                throw new Exception("Сервер отключился");

            NetTCP.SendMessage($"GetFile{elementPath}");

            byte[] msg = NetTCP.GetMessage();

            if (Encoding.UTF8.GetString(msg) == "")
                throw new Exception("Ошибка при получении файла");

            System.IO.File.WriteAllBytes(elementPath.Replace(_userPath + "\\", ""), msg);
        }

        public string GetBackPath()
        {
            string[] arrayDirectory = _userPath.Split('\\');
            int counter = _userPath.Count(x => (x == '\\'));
            string backPath = "";
            for (int i = 0; i < counter; i++)
            {
                backPath += arrayDirectory[i];
                if (i == counter - 1)
                    break;
                backPath += '\\';
            }
            if (backPath == "ServerDirectory\\")
                backPath = "";

            return backPath;
        }
    }
}


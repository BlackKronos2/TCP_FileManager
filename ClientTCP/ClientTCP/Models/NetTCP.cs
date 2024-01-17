using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTCP
{
    public static class NetTCP
    {
        private static Socket client;
        public static Socket Connection(string ipAddress)
        {
            IPAddress ipAddr = IPAddress.Parse(ipAddress);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 58000);
            client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEndPoint);
            return client;
        }

        public static void SendMessage(string message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            int bytesSent = client.Send(msg);
        }

        public static byte[] GetMessage()
        {
            byte[] bytes = new byte[1024]; // Буфер для входящих данных
            int bytesRec = client.Receive(bytes);
            byte[] responseData = new byte[bytesRec];
            Buffer.BlockCopy(bytes, 0, responseData, 0, bytesRec); //запись в bytes
            return bytes;
        }

    }
}

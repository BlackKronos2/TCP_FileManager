using ServerTCPTest;
using System.Net;
using System.Net.Sockets;

class Program
{
    public static TCPServer server = new TCPServer();

    static void Main(string[] args)
    {
        server.ServerStart();
    }
}

using ServerTCPTest;
using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
		Console.WriteLine("Введите IP-адрес для сервера: ");
		string input = Console.ReadLine();
		TCPServer server;

		try
		{
			server = new TCPServer(input);
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
    }
}

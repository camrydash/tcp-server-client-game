using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumberGuessingGame.Server
{
    internal class GameServer
    {
        private TcpListener _listener;
        private readonly IPAddress _IPAddress;
        private readonly int _port;
        private readonly List<string> _clients = new List<string>();

        internal GameServer(IPAddress IPAddress, int port)
        {
            _IPAddress = IPAddress;
            _port = port;
        }

        internal void Start()
        {
            try
            {
                if (_listener == null)
                {
                    _listener = new TcpListener(_IPAddress, _port);
                }

                _listener.Start();

                Listen();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        private void Listen()
        {
            Console.WriteLine("Server Initiated @ {0}:{1}", _IPAddress, _port);
            Console.WriteLine("Listening...");

            while (true)
            {
                if (_listener.Pending())
                {
                    TcpClient connection = _listener.AcceptTcpClient();
                    GameConnectionHandler connectionHandler = new GameConnectionHandler(connection);
                    _clients.Add(connectionHandler.ClientId);
                }
                Thread.Sleep(1000);
            }        
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumberGuessingGame.Client
{
    internal class GameClientConsumer
    {
        private readonly string _IPAddress;
        private readonly int _port;
        private readonly TcpClient _client = new TcpClient();
        private bool _isConnected = true;

        internal GameClientConsumer(string IPAddress, int port)
        {
            _IPAddress = IPAddress;
            _port = port;
        }

        internal void Start()
        {
            try
            {
                if (!_client.Connected)
                    _client.Connect(_IPAddress, _port);

                if (_client.Connected)
                {
                    Thread receiver = new Thread(new ThreadStart(Receive));
                    receiver.Start();

                    string clientConnectionId = Guid.NewGuid().ToString();

                    Send(clientConnectionId);

                    Listen();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        internal void Listen()
        {
            string commandKey = null;
            while (commandKey != "quit")
            {
                commandKey = Console.ReadLine();
                Send(commandKey);
            }
        }

        internal void Send(string message)
        {
            if (_client.Connected)
            {
                StreamWriter writer = new StreamWriter(_client.GetStream());
                writer.WriteLine(message);
                writer.Flush();
            }
            else
            {
                throw new ApplicationException("Connection closed.");
            }
        }

        internal void Receive()
        {
            StreamReader reader = new StreamReader(_client.GetStream());
            while (_client.Connected)
            {
                try
                {
                    string message = reader.ReadLine();
                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(50);
            }
        }

        internal void Close()
        {
            _client.Close();
        }

        public bool Connected
        {
            get { return _client.Connected; }
        }
    }
}

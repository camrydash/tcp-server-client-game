using NumberGuessingGame.Domain;
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
    class Program
    {
        private static int DefaultPortNumber = 5556;

        static void Main(string[] args)
        {
            IPAddress IPAddress = NetworkHelper.GetLocalIPAddress();
            if (IPAddress == null)
                throw new ArgumentException("Unable to find local IP address");

            GameServer server = new GameServer(IPAddress, DefaultPortNumber);
            server.Start();
            ///Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuessingGame.Client
{
    class Program
    {
        private static int DefaultPortNumber = 5556;

        static void Main(string[] args)
        {
            IPAddress IPAddress = null;
            if (args.Length > 0)
            {
                try
                {
                    IPAddress = IPAddress.Parse(args[0]);
                }
                catch
                {
                    Console.WriteLine("Could not decode IP address: {0}", args[0]);                   
                }
            }

            if (IPAddress == null) 
                IPAddress = NetworkHelper.GetLocalIPAddress();

            if (IPAddress != null)
            {
                GameClientConsumer clientConsumer = new GameClientConsumer(IPAddress.ToString(), DefaultPortNumber);
                clientConsumer.Start();
            }
        }
    }
}

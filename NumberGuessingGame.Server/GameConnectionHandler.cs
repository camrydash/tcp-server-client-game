using NumberGuessingGame.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumberGuessingGame.Server
{
    internal class GameConnectionHandler
    {
        private NumberGame _numberGame;
        private readonly TcpClient _connection;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private bool _isDisconnected;

        public GameConnectionHandler(TcpClient gameConnection)
        {
            _connection = gameConnection;
            _reader = new StreamReader(_connection.GetStream());
            _writer = new StreamWriter(_connection.GetStream());

            Thread clientInstance = new Thread(new ThreadStart(InitializeConnection));
            clientInstance.Start();  
        }

        private void InitializeConnection()
        {
            InitializeGame();
            ListenGame();
        }

        private void InitializeGame()
        {
            if (ClientId == null)
            {
                ClientId = ReceiveClientResponse();
                Console.WriteLine("Client: {0} Connected.", ClientId);
            }

            DisplayWelcomeMessage();
            GetGameParameters();
        }

        private void ListenGame()
        {
            while (!_isDisconnected)
            {
                try
                {
                    _numberGame.InputInstructions();

                    var inputValue = ReceiveClientResponse();
                    int parsedValue;
                    if (int.TryParse(inputValue, out parsedValue))
                    {
                        if (_numberGame.TryGuessValue(parsedValue))
                        {
                            //start all over again
                            InitializeConnection();
                        }
                    }
                    else
                    {
                        _writer.WriteLine("Invalid input.");
                        _writer.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        public void DisplayWelcomeMessage()
        {
            _writer.WriteLine("Welcome to the number guessing game.");
            _writer.WriteLine("Type 'quit' to leave the game.");
            _writer.WriteLine("Please enter a lower and upper limit for this game: Example => 10 20 ");
            _writer.Flush();
        }

        private void GetGameParameters()
        {
            //defaults
            int lowerBound = 0, upperBound = 20;
            try
            {
                if (!int.TryParse(ReceiveClientResponse(), out lowerBound) || !int.TryParse(ReceiveClientResponse(), out upperBound))
                {
                    _writer.WriteLine("Invalid input. Using Defaults (0, 20)");
                    _writer.Flush();
                }

                _numberGame = new NumberGame(lowerBound, upperBound, _writer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Receives value from the client synchronously
        /// </summary>
        /// <returns></returns>
        private string ReceiveClientResponse()
        {
            char[] buffer = new char[36];
            int bytesRead = 0;

            try
            {
                // Block till client sends some data.
                bytesRead = _reader.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    throw new ApplicationException("Client disconnected.");
                }
            }
            catch
            {
                _isDisconnected = true;
                throw new ApplicationException("Connection closed.");
            }

            return new string(buffer).Replace("\r\n", string.Empty).Replace("\0", string.Empty);
        }

        public string ClientId { get; private set; }
    }
}

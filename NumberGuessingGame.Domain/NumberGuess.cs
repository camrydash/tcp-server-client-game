using System;
using System.Collections.Generic;
using System.IO;

namespace NumberGuessingGame.Domain
{
    public class NumberGame
    {
        private readonly TextWriter _writer;
        private readonly int _lowerBound;
        private readonly int _upperBound;
        private readonly List<int> _guessHistory = new List<int>();
        private readonly int _correctValue;

        public NumberGame(int lowerBound, int upperBound, TextWriter writer)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            _writer = writer;

            _correctValue = CreateRandomNumber(lowerBound, upperBound);
        }

        public void InputInstructions()
        {
            _writer.WriteLine("Please guess a number between {0} and {1}:", _lowerBound, _upperBound);
            _writer.Flush();
        }

        public bool TryGuessValue(int value)
        {
            var result = false;
            try
            {
                _guessHistory.Add(value);

                if (value > _correctValue)
                {
                    _writer.WriteLine("Your guess of {0} is higher than the answer", value);
                }
                else if (value < _correctValue)
                {
                    _writer.WriteLine("Your guess of {0} is lower than the answer", value);
                }
                else
                {
                    _writer.WriteLine("Congratulations: {0} is the coorect answer.", value);
                    _writer.WriteLine("It took you {0} guesses to reach the correct answer", NumberOfGuesses);
                    _writer.WriteLine("Your guesses were: {0}\r\n", GuessFormattedHistory);
                    result = true;
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _writer.Flush();                
            }
            return result;
        }

        private int CreateRandomNumber(int lowerBound, int upperBound)
        {
            return (new Random().Next(lowerBound, upperBound));
        }

        private int NumberOfGuesses { get { return _guessHistory.Count; } }
        private string GuessFormattedHistory { get { return string.Join(", ", _guessHistory); } }
    }
}
using System;

namespace Calculator
{
    class Program
    {
        static double Percent(double number) 
        => number / 0.1d / 1000d;
        
        static double PowerOfNumber(double baseNumber, double powerNumber)
        {
            double results = baseNumber * 1;

            for (int doubleCount = 1; // Doubles results each iteration based on the power number
            doubleCount < powerNumber; doubleCount++)
            {
                results = results * baseNumber;
            }
            return results;
        }

        static void MultiplyAndDivide(string input, int at) // TODO: FINISH WORKING ON THIS FUNCTION
        {
            double tmpNumber = 0.0d;
            
            for (; at < input.Length; at++)
            {
                if (input[at] == '*')
                {
                    tmpNumber += GetLastNumber(input, at--);
                    tmpNumber *= GetNumber(input, at++);
                }
            }
        }

        static void AddAndSubtract(string input, int at)
        {   
            double tmpNumber = 0.0d;

            for (; at < input.Length-1; at++)
            {
                if (input[at] == '+')
                   { 
                        tmpNumber += GetLastNumber(input, at--);
                        tmpNumber += GetNumber(input, at++);
                   }

                else if (input[at] == '-')
                    tmpNumber -= GetNumber(input, at++);
            }
        }
        
        static double GetNumber(string input, int at)
        {
            int startIndex = at;
            int numberLength = 0;
            
            if (input[at] == '-')
                numberLength++;
            
            for (; at < input.Length; at++)
            {
                if (input[at] >= '0' || input[at] <= '9' || input[at] == '.') 
                    numberLength++;
                else break;
            }

            if (numberLength == 0)
                throw new System.ArgumentException("GetNumber() You can't have zero number length");

            return Convert.ToDouble(input.Substring(startIndex, numberLength));
        }

        static double GetLastNumber(string input, int at)
        {
            int startIndex = at;
            int numberLength = 0;

            for (; at >= 0; at--)
            {
                if (input[at] >= '0' || input[at] <= '9' || input[at] == '.')
                   { 
                       startIndex = at;
                       numberLength++;
                    }
                
                else if (input[at] == '-')
                {
                    if (at == 0)
                    {
                        startIndex = at;
                        numberLength++;
                        break;
                    }
                    
                    else if (input[at-1] == '-' || input[at-1] !>= '0' || input[at-1] !<= '9')
                    {
                        startIndex = at;
                        numberLength++;
                        break;
                    }
                    else break;
                }
                else break;
            }

            if (numberLength == 0)
                throw new System.ArgumentException("GetLastNumber() You can't have zero number length");

            return Convert.ToDouble(input.Substring(startIndex, numberLength));
        }

        static void Main()
        {   
            string userInput = Console.ReadLine();
            userInput = String.Concat(userInput.Split(' ')); // Removes all the whitespace from the user's input.
            
            MultiplyAndDivide(userInput, 0);
            AddAndSubtract(userInput, 0);
        }
    }
}
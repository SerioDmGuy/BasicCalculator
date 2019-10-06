using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCalculator
{
    class Program
    {


        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Basic Calculator!");
            Console.WriteLine("");
            
            // Stores the first number when received/entered
            Console.Write("first number = ");

            string checkFirstNumber = Console.ReadLine();

            // Converts the user's input into usable number
            int firstNumber = Convert.ToInt32(checkFirstNumber);

            //Makes another line spacing them out
            Console.WriteLine("");


            Console.WriteLine("How shall I calculate? ");
            Console.WriteLine("+");
            Console.WriteLine("-");
            Console.WriteLine("/");
            Console.WriteLine("*");

            Console.Write("calculate using this: ");

            //Lets the program know what type of calculation the user as requested
            string calculateType = Console.ReadLine();

            //Creates another line to space out the layout
            Console.WriteLine("");


            Console.Write("second number = ");

            string checkSecondNumber = Console.ReadLine();

            int secondNumber = Convert.ToInt32(checkSecondNumber);


            switch (calculateType)
            {

                case "+":
                    Console.WriteLine("");
                    Console.Write("Total = ");
                    Console.WriteLine(firstNumber + secondNumber);
                    break;

                case "-":
                    Console.WriteLine("");
                    Console.Write("Total = ");
                    Console.WriteLine(firstNumber - secondNumber);
                    break;

                case "/":
                    Console.WriteLine("");
                    Console.Write("Total = ");
                    Console.WriteLine(firstNumber / secondNumber);
                    break;

                case "*":
                    Console.WriteLine("");
                    Console.Write("Total = ");
                    Console.WriteLine(firstNumber * secondNumber);
                    break;
            }

            // Asks the user to exit the console when finished
            Console.WriteLine("");
            Console.WriteLine("Please close the window or press 'Enter' to exit the program");
            Console.WriteLine("Created by Luke aka SerioDmGuy");
            Console.ReadKey();

        }
    }
}

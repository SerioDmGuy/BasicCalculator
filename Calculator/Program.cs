using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {    
        // Math Operators {"-", "+", "/", "*", "%", "mod", "**", "^",};
        struct MathOperator
        {
            string value {get; set;}
            int precedence {get; set;}
            string associativity {get; set;}
        }

        
        // TODO: CREATE INSTANCES OF MATH OPERATORS!!!



        // NOTE: Math operators are ordered by precedence (lowest to highest elements)
        static readonly private string[] mathOperators = {"-", "+", "/", "*", "%", "mod", "**", "^",};
        // TODO: Add alt operators called "modulo", "modulus" for our modulo operation.
        // TODO: Does a operator require one or two numbers? as a variabile. 
        
        // ----------------------------------------------------------
        // TODO ASAP: Create operator struct with a precedence variable.
        // ----------------------------------------------------------

        static bool IsMathOperator(String str)
        {   
            foreach (String mathOperator in mathOperators)
            {
                if (mathOperator == str)
                    return true;
            } 
            return false;
        }

        static bool IsMathOperatorHigherPrecedence(String yourOperator, String comparedOperator)
        {            
            // NOTE: Math operators are ordered by element precedence
            List<String> listOperators = new List<String>(mathOperators);
            int yourOperatorPrecedence = listOperators.IndexOf(yourOperator);
            int comparedOperatorPrecedence = listOperators.IndexOf(comparedOperator);

            if (yourOperatorPrecedence > comparedOperatorPrecedence) return true;
            return false;
        }

        static bool IsNumber(string str)
        {
            if (str == "NaN") 
                return false;
            
            if (Double.TryParse(str, out double foundNumber)) 
                return true;
            
            return false; 
        }

        static bool IsParenthesis(string str)
        {
            if (str == "(" || str == ")") return true;
            return false;
        }

        static bool IsOnlyOneParenthesisPopped(bool openParenthesisPop, bool closeParenthesisPop)
        {       
            if (openParenthesisPop && !closeParenthesisPop)
            {     
                Console.WriteLine("ERROR: Expected closed parenthesis");
                return true;
            }

            if (!openParenthesisPop && closeParenthesisPop)
            {
                Console.WriteLine("ERROR: Expected open parenthesis"); 
                return true;
            }
            
            return false;
        }

        static bool IsEmpty<T>(Stack<T> stack)
        {
            if (stack.Count <= 0) return true;
            return false;
        }

        static string PeekOrProvideElement(Stack<Match> stack, string provideElement)
        {
            if (IsEmpty<Match>(stack))
                return provideElement;
            
            return stack.Peek().Value;
        }
        
        static void PopOnly(String thisElement, out bool isPopped, Stack<Match> stack)
        {
            // TODO: PopOnly should also return the element like regular pop can
            isPopped = false;
            
            // TODO: Throw an empty exception if the stack itself is empty like regular Pop() can
            if (IsEmpty<Match>(stack)) 
                return;
            
            if (stack.Peek().Value == thisElement)
            {
                stack.Pop(); 
                isPopped = true;
            }
        }

        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            // Creates infix tokens from a infix expression.
            // We spilt up the infix expression into tokens using regex
            // matching either numbers, parentheses or supported math operators.
            
            // TODO: Split Regex into their own strings rather than a massive one liner
            // TODO: Check for invalid characters!!!
            Regex infixTokens = new Regex(@"(?<FindNumbers>(?=[-]\d[.]|[-][.]\d|[.]\d|[-]\d|\d)(?!(?<=\d[.]|\d)[-](?=[.]?\d+))[-]?\d*[.]?\d*([%](?![.][\d]|[\d][.]|[-]|\d))?)|(?<FindMathOperators>[*]{2}|[()\/*+%^-]|mod)");
            return infixTokens.Matches(infixExpression);
        }

        static Queue<Match> InfixToPostfixTokens(MatchCollection infixTokens)
        {   
            // Makes postfix tokens using the "Shunting yard algorithm".
            // Essentially reordering infix tokens by using a queue and a stack,
            // we can efficiently create better evaluation tokens.
            
            Queue<Match> postfixTokens = new Queue<Match>();
            Stack<Match> operatorStack = new Stack<Match>();
            
            foreach (Match token in infixTokens)
            {
                if (IsNumber(token.Value)) 
                    postfixTokens.Enqueue(token);
                
                else if (IsParenthesis(token.Value)) 
                {
                    operatorStack.Push(token);
                    
                    if (")" == token.Value) 
                        // We assume a parenthesis pair as been detected
                        // therefore we enqueue maths opearators from our stack
                        EnqueueMathOperators(ref postfixTokens, ref operatorStack);
                }

                else if (IsMathOperator(token.Value))
                {   
                    if (!IsMathOperatorHigherPrecedence(token.Value, PeekOrProvideElement(operatorStack, token.Value)))
                        EnqueueMathOperators(ref postfixTokens, ref operatorStack);
                    operatorStack.Push(token);
                }
                
                else 
                { 
                    Console.Write("ERROR: Invalid infix token or tokens detected during postfix token creation. ");
                    Console.WriteLine("\n Make sure your infix expression is supported by Basic Calculator");
                    postfixTokens = null;
                }

                if (postfixTokens == null)
                {
                    Console.WriteLine("ERROR: Postfix token creation has been terminated."); 
                    return postfixTokens = null;
                }
            }
            
            // All math operators except multiple parenthesis pairs 
            // get enqueued from the stack at the end.
            EnqueueMathOperators(ref postfixTokens, ref operatorStack);

            if (!IsEmpty(operatorStack)) 
                Console.WriteLine("ERROR: Leftover tokens within the postfix operator stack aren't enqueued.");
            // TODO: If leftovers are detected exception occurs and terminates this function  

            return postfixTokens;
        }

        static Queue<Match> EnqueueMathOperators(ref Queue<Match> yourQueue, ref Stack<Match> mathOperators)
        {   
            PopOnly(")", out bool isCloseParenthesisPop, mathOperators);
            
            while (!IsEmpty(mathOperators))
            {          
                if (IsMathOperator(mathOperators.Peek().Value))
                    yourQueue.Enqueue(mathOperators.Pop());
                else break;
            }
            
            if (isCloseParenthesisPop)
            { 
                PopOnly("(", out bool isOpenParenthesisPop, mathOperators);

                if (IsOnlyOneParenthesisPopped(isOpenParenthesisPop, isCloseParenthesisPop))
                { 
                    Console.WriteLine("ERROR: Open parenthesis missing from your parenthesis pair.");
                    return yourQueue = null; 
                }
            }

            return yourQueue;
        } 

        static Double EvaluatePostfixTokens(Queue<Match> postfixTokens)
        {
            if (postfixTokens == null)
            {
                Console.WriteLine("ERROR: Postfix tokens wasn't acquired for evaluation."); 
                return 0;
            }

            Stack<Double> numberStack = new Stack<Double>();

            foreach (Match token in postfixTokens)
            {
                if (IsNumber(token.Value))
                {
                    numberStack.Push(Convert.ToDouble(token.Value));
                    continue;
                }

                if (IsMathOperator(token.Value))
                {
                    numberStack.Push(EvaluateMathOperator(ref numberStack, token.Value)); 
                    continue;
                }

                Console.WriteLine($"ERROR: Can't evaluate a non-postfix token '{token.Value}'");
                return 0;
            }           
            
            return GetNumber(numberStack);
        }

        static double EvaluateMathOperator(ref Stack<Double> yourNumbers, String mathOperator)
        {
            if (yourNumbers.Count < 2)
            {
                Console.WriteLine($"ERROR: Can't evaluate a maths operator without at least two numbers."); 
                return 0.0;
            }
            
            double secondNumber = yourNumbers.Pop();
            double firstNumber = yourNumbers.Pop();

            // TODO: Check both firstNumber and Second for NaN "use isNumber() for checking this error"

            switch (mathOperator)
            {   
                case "mod": return GetEuclideanModulo(firstNumber, secondNumber);
                case "%": return GetEuclideanModulo(firstNumber, secondNumber);
                case "^": return GetPowerOfNumber(firstNumber, secondNumber);
                case "**": return GetPowerOfNumber(firstNumber, secondNumber);
                case "*": return (firstNumber * secondNumber);
                case "/": return GetDivide(firstNumber, secondNumber);
                case "+": return (firstNumber + secondNumber);
                case "-": return (firstNumber - secondNumber);
                default: Console.Write($"ERROR: Cannot evaluate because it's not a supported math operator '{mathOperator}'"); return 0.0d;
            }
        }

        static double GetDivide(double dividend, double divisor)
        {
            if (divisor == 0) 
            {
                Console.WriteLine("ERROR: You can't divide by zero");
                return double.NaN;
            }

            return dividend / divisor;
        }

        static double GetEuclideanModulo(double dividend, double divisor)
        {
            // Returns the remainder of a Euclidean division with factional parts.
            
            // TODO: Remainder should include the factional number as well
            // For example 2.5mod2 = 0.5
            
            Math.DivRem((int) dividend, (int) divisor, out int remainder);
            return remainder;
        }

        static double GetPowerOfNumber(double baseNumber, double powerNumber)
        {
            double results = Math.Pow(baseNumber, powerNumber);
            
            if (IsNumber($"{results}")) 
                return results;
            
            else Console.WriteLine($"ERROR: This value '{results}' isn't a number and can't be POWed"); 
            return double.NaN;
        }

        static double GetNumber(Stack<Double> yourStack)
        {
            // Gets a number from a stack and returns it as a result.
            // TODO: Rewrite identifier to SetResults?
            
            if (yourStack.Count >= 1)
            {
                if (IsNumber(yourStack.Peek().ToString())) 
                    return yourStack.Pop();

                Console.WriteLine("ERROR: Results must be a number"); 
                return Double.NaN;
            }
            
            return 0;
        }

        static void Main()
        {   
            // Upon start up it switches on the calcaultor and asks the user for input.
            // You can input a infix expression, clear screen or even exit out the program!
        
            bool calculatorOn = true;

            while (calculatorOn)
            {
                string userInput = String.Concat(Console.ReadLine().ToLower().Split(" "));
                
                while (calculatorOn) 
                {
                    // TODO: Abstract these commands into a another function for readability
                    if (userInput == "exit")
                    {
                        calculatorOn = false; 
                        break;
                    }
                    
                    if (userInput == "clear" || userInput == "cls")
                    {
                        Console.Clear(); 
                        break;
                    }
                    
                    if (String.IsNullOrEmpty(userInput) || String.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("ERROR: Your input must not be empty."); 
                        break;
                    }
                    
                    Console.WriteLine($"Equals: {EvaluatePostfixTokens(InfixToPostfixTokens(CreateInfixTokens(userInput)))}");
                    // TODO: Abstract this line above into it's own function for readability sake.  
                    break;
                }
            }
        }
    }
}
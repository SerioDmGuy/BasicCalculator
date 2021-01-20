using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {    
        static readonly private char[] listMathOperators = {'-', '+', '/', '*', '^'};
        
        static bool IsMathOperator(String yourOperator)
        {
            if (yourOperator.Length > 1)
                return false;
            
            foreach (Char Operator in listMathOperators)
            {
                if (Operator == Convert.ToChar(yourOperator))
                    return true;
            } 
            return false;
        }

        static bool IsMathOperatorHigherPrecedence(String yourOperator, String comparedOperator)
        {            
            List<Char> listOperators = new List<Char>(listMathOperators);
            int yourOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(yourOperator));
            int comparedOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(comparedOperator));
            
            if (yourOperatorPrecedence > comparedOperatorPrecedence) return true;
            else return false;
        }

        static bool IsStackEmpty<T>(Stack<T> yourStack)
        {
            if (yourStack.Count <= 0) return true;
            else return false;
        }

        static bool IsParenthesis(string yourInput)
        {
            if (yourInput == "(" || yourInput == ")") return true;
            else return false;
        }

        static bool IsNumber(string yourInput)
        {
            if (Double.TryParse(yourInput, out double foundNumber)) return true;
            else return false; 
        }

        static string PeekOrProvideElement(Stack<Match> yourStack, string provideElement)
        {
            if (IsStackEmpty<Match>(yourStack))
                return provideElement;
            else return yourStack.Peek().Value;   
        }
        
        static void PopOnly(String element, out bool isPopped, Stack<Match> yourStack)
        {
            isPopped = false;
            
            if (IsStackEmpty<Match>(yourStack)) {}
            else if (yourStack.Peek().Value == element)
            {
                yourStack.Pop(); 
                isPopped = true;
            }
        }

        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            infixExpression = String.Concat(infixExpression.Split(' ')); // TODO: Create a function that removes all whitespaces from expressions for calling instead.
            
            Regex infixTokens = new Regex(@"(?<FindSubtraction>(?<=[)])[-])|(?<FindNumbers>(?!(?<=\d)[-](?=[.]?\d+))[-]?\d*[.]?\d+)|(?<FindOperators>[()\/*+^-])|(?<IncludeInvalidTokens>.)");
            return infixTokens.Matches(infixExpression);
        }

        static Queue<Match> CreatePostfixTokens(MatchCollection infixTokens)
        {   
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
                        EnqueuePostfixTokensFromStack(ref postfixTokens, ref operatorStack);
                }

                else if (IsMathOperator(token.Value))
                {   
                    if (!IsMathOperatorHigherPrecedence(token.Value, PeekOrProvideElement(operatorStack, token.Value)))
                        EnqueuePostfixTokensFromStack(ref postfixTokens, ref operatorStack);
                    operatorStack.Push(token);
                }
                else 
                { 
                    Console.Write("ERROR: Invalid infix token or tokens detected during postfix token creation, ");
                    Console.WriteLine("make sure your infix expression is supported by Basic Calculator");
                    return null;
                }
            }
            EnqueuePostfixTokensFromStack(ref postfixTokens, ref operatorStack);

            if (!IsStackEmpty(operatorStack)) 
            {   
                Console.Write("ERROR: Your infix expression isn't correct, ");
                Console.WriteLine("make sure your operators are in the right positions."); 
                return null; 
            }
            return postfixTokens;
        }

        static Queue<Match> EnqueuePostfixTokensFromStack(ref Queue<Match> yourQueue, ref Stack<Match> operatorStack)
        {   
            PopOnly(")", out bool closedParenthesisPopped, operatorStack);
            
            while (!IsStackEmpty(operatorStack))
            {          
                if (IsParenthesis(operatorStack.Peek().Value))
                    break;
                else if (IsMathOperator(operatorStack.Peek().Value))
                    yourQueue.Enqueue(operatorStack.Pop());
                else 
                    {Console.WriteLine("ERROR: Detected an non-operator token that can't be enqueued."); break;}
            }

            PopOnly("(", out bool openParenthesisPopped, operatorStack);

            if (openParenthesisPopped)
            { 
                if (!closedParenthesisPopped) 
                { Console.WriteLine("ERROR: Expected closed parentheses"); yourQueue = null; }
            }

            if (closedParenthesisPopped)
            { 
                if (!openParenthesisPopped) 
                { Console.WriteLine("ERROR: Expected open parentheses"); yourQueue = null; }
            }

            return yourQueue;
        } 

        static Double EvaluatePostfixTokens(Queue<Match> postfixTokens)
        {
            if (postfixTokens == null)
            {Console.WriteLine("ERROR: Postfix tokens wasn't acquired for evaluation."); return 0.0;}

            Stack<Double> numberStack = new Stack<Double>();

            while (postfixTokens.Count >= 1)
            {
                if (IsNumber(postfixTokens.Peek().Value))
                    numberStack.Push(Convert.ToDouble(postfixTokens.Dequeue().Value));

                else if (IsMathOperator(postfixTokens.Peek().Value))
                    numberStack.Push(GetOperatorExpressionResults(ref numberStack, postfixTokens.Dequeue().Value));
                
                else 
                { 
                    Console.Write($"ERROR: Detected unsupported token '{postfixTokens.Peek()}' can't be processed ");
                    Console.WriteLine("(Numbers or operator tokens only) for Postfix Token Evaluator.");
                    return 0.0;
                }
            }            
            return GetResultsFromNumberStack(numberStack);
        }

        static double GetOperatorExpressionResults(ref Stack<Double> numberStack, String yourOperator)
        {
            if (numberStack.Count < 2)
            {Console.WriteLine("ERROR: Can't evaluate your operator without two numbers."); return 0.0;}
            
            double secondNumber = numberStack.Pop();
            double firstNumber = numberStack.Pop();

            switch (yourOperator)
            {   
                case "*": return (firstNumber * secondNumber);
                case "/": return (firstNumber / secondNumber);
                case "+": return (firstNumber + secondNumber);
                case "-": return (firstNumber - secondNumber);
                case "^": return GetPowerOfNumber(firstNumber, secondNumber);
                default: Console.WriteLine($"ERROR: Operator token '{yourOperator}' cannot evaluate because it's not supported."); return 0.0d;
            }
        }

        static double GetPercent(double number)
        => number / 0.1d / 1000d;
        
        static double GetPowerOfNumber(double baseNumber, double powerNumber)
        {
            double results = baseNumber * 1;

            for (int powerCount = 1;
            powerCount < powerNumber; powerCount++)
               { results = results * baseNumber; }
            
            return results;
        }

        static double GetResultsFromNumberStack(Stack<Double> yourStack)
        {
            if (yourStack.Count <= 0)
                {Console.WriteLine("ERROR: Can't create results unless a number is given."); return 0.0d;}
        
            else if (yourStack.Count >= 2)
                {Console.WriteLine("ERROR: Left over numbers weren't evaluated"); return 0.0d;}

            return yourStack.Pop();
        }

        static void Main()
        {   
            bool calculatorOn = true;
            
            while (calculatorOn)
            {
                string userInput = Console.ReadLine();
                
                while (calculatorOn) 
                {
                    // TODO: Function that handles all commands from input,
                    // along with Upper and lowercase support.
                    // --------------------------------------
                    if (userInput == "exit")
                        {calculatorOn = false; break;}
                    
                    else if (userInput == "clear" || userInput == "cls")
                        {Console.Clear(); break;}
                    // --------------------------------------
                    
                    else if (String.IsNullOrEmpty(userInput) || String.IsNullOrWhiteSpace(userInput))
                        {Console.WriteLine("ERROR: Your input must not be empty."); break;}
                    
                    Console.WriteLine($"Equals: {EvaluatePostfixTokens(CreatePostfixTokens(CreateInfixTokens(userInput)))}");
                    break;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {    
        static private char[] listMathOperators = {'-', '+', '/', '*',};
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

        static bool IsNumber(string str)
        {
            if (Double.TryParse(str, out double foundNumber)) return true;
            else return false; 
        }
        
        static bool IsOperator(Match yourOperator)
        {
            if (yourOperator.Value.Length > 1)
                return false;
            
            foreach (Char pmdasOperator in listMathOperators)
            {
                if (pmdasOperator == Convert.ToChar(yourOperator.Value))
                    return true;
            } 
            return false;
        }

        static bool IsOperatorHigherPrecedence(Match yourOperator, Match comparedOperator)
        {            
            List<Char> listOperators = new List<Char>(listMathOperators);
            int yourOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(yourOperator.Value));
            int comparedOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(comparedOperator.Value));
            
            if (yourOperatorPrecedence > comparedOperatorPrecedence) return true;
            else return false;
        }

        static Queue<Match> EnqueueOperatorStackTokensExceptParentheses(ref Queue<Match> yourQueue, ref Stack<Match> operatorStack)
        {   
            // TODO: Clean up function by splitting up "starts with closed parenthesis" 
            //  and "ends in open parenthesis" into their own functions
            
            bool startsWithClosedParenthesis = false;
            if (operatorStack.Count >= 1 && operatorStack.Peek().Value == ")")
            { 
                operatorStack.Pop(); 
                startsWithClosedParenthesis = true;
            }
        
            while (operatorStack.Count >= 1)
            {          
                if (operatorStack.Peek().Value == "(" || operatorStack.Peek().Value == ")")
                    break;
                else if (IsOperator(operatorStack.Peek()))
                    yourQueue.Enqueue(operatorStack.Pop());
                else 
                    {Console.WriteLine("ERROR: Detected an non-operator token that can't be enqueued."); break;}
            }

            if (startsWithClosedParenthesis)
            {
                operatorStack.TryPeek(out Match lastOperator);
                
                if (operatorStack.Count >= 1 && lastOperator.Value == "(")
                    operatorStack.Pop();
                else 
                {   Console.WriteLine("ERROR: All closed parentheses must have a open parenthesis"); 
                    yourQueue = null;
                }
            }
            return yourQueue;
        }  
        
        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            infixExpression = String.Concat(infixExpression.Split(' ')); // TODO Create a function that removes all whitespaces from expressions for calling instead.
            
            Regex createInfixTokens = new Regex(@"(?<FindSubtraction>(?<=[)])[-])|(?<FindNumbers>(?!(?<=\d)[-](?=[.]?\d+))[-]?\d*[.]?\d+)|(?<FindOperators>[()\/*+-])|(?<IncludeInvalidTokens>.)");
            return createInfixTokens.Matches(infixExpression);
        }

        static Queue<Match> CreatePostfixTokens(MatchCollection infixTokens)
        {   
            Queue<Match> postfixTokens = new Queue<Match>();
            Stack<Match> operatorStack = new Stack<Match>();
            
            foreach (Match token in infixTokens)
            {
                if (IsNumber(token.Value)) 
                    postfixTokens.Enqueue(token);
                
                else if ("(" == token.Value) 
                    operatorStack.Push(token);

                else if (")" == token.Value)
                { 
                    operatorStack.Push(token);
                    EnqueueOperatorStackTokensExceptParentheses(ref postfixTokens, ref operatorStack);
                }

                else if (IsOperator(token))
                {   
                    if (operatorStack.Count >= 1 && !IsOperatorHigherPrecedence(token, operatorStack.Peek()))
                        EnqueueOperatorStackTokensExceptParentheses(ref postfixTokens, ref operatorStack);
                    operatorStack.Push(token);
                }
                else 
                { 
                    Console.Write("ERROR: Invalid infix token or tokens detected during postfix token creation, ");
                    Console.WriteLine("make sure your infix expression is supported by Basic Calculator");
                    return null;
                }
            }
            EnqueueOperatorStackTokensExceptParentheses(ref postfixTokens, ref operatorStack);

            if (operatorStack.Count >= 1) 
            {   
                Console.Write("ERROR: Your infix expression isn't correct, ");
                Console.WriteLine("make sure your operators are in the right positions."); 
                return null; 
            }
                
            return postfixTokens;
        }

        static Double EvaluatePostfixTokens(Queue<Match> postfixTokens)
        {
            Stack<Double> numberStack = new Stack<Double>();
            
            if (postfixTokens == null)
            { Console.WriteLine("ERROR: Postfix tokens wasn't acquired for evaluation."); return 0.0; }

            while (postfixTokens.Count >= 1)
            {
                if (IsNumber(postfixTokens.Peek().Value))
                    numberStack.Push(Convert.ToDouble(postfixTokens.Dequeue().Value));

                else if (IsOperator(postfixTokens.Peek()))
                    { 
                        if (numberStack.Count >= 2)
                        numberStack.Push(EvaluateOperatorExpression(ref numberStack, postfixTokens.Dequeue().Value));
                        else {Console.WriteLine("ERROR: Can't evaluate your operator without two numbers."); return 0.0;}
                    }
                 
                else 
                { 
                    Console.Write($"ERROR: Detected unsupported token '{postfixTokens.Peek().Value}' can't be processed ");
                    Console.WriteLine("(Numbers or operator tokens only) for Postfix Token Evaluator.");
                    return 0.0;
                }
            }            
            return GetResultsFromNumberStack(numberStack);
        }

        static double EvaluateOperatorExpression(ref Stack<Double> numberStack, String yourOperator)
        {
            if (numberStack.Count < 2)
            {Console.WriteLine("ERROR: Can't evaluate operator without two numbers."); return 0.0d;}
            
            double secondNumber = numberStack.Pop();
            double firstNumber = numberStack.Pop();

            switch (yourOperator)
            {   
                case "*": return (firstNumber * secondNumber);
                case "/": return (firstNumber / secondNumber);
                case "+": return (firstNumber + secondNumber);
                case "-": return (firstNumber - secondNumber);
                default: Console.WriteLine($"ERROR: Operator token '{yourOperator}' cannot evaluate because it's not supported."); return 0.0;
            }
        }

        static double GetResultsFromNumberStack(Stack<Double> numberStack)
        {
            if (numberStack.Count <= 0)
                {Console.WriteLine("ERROR: Can't create results unless a number is given."); return 0.0d;}
        
            else if (numberStack.Count >= 2)
                {Console.WriteLine("ERROR: Left over numbers weren't evaluated"); return 0.0d;}

            return numberStack.Pop();
        }

        static void Main()
        {   
            bool calculatorOn = true;
            
            while (calculatorOn)
            {
                string userInput = Console.ReadLine();
                
                while (calculatorOn) 
                {
                    if (userInput == "exit") 
                        {calculatorOn = false; break;}
                    
                    else if (userInput == "clear" || userInput == "cls")
                        {Console.Clear(); break;}
                    
                    else if (String.IsNullOrEmpty(userInput) || String.IsNullOrWhiteSpace(userInput))
                        {Console.WriteLine("ERROR: Your input must not be empty."); break;}
                    
                    Console.WriteLine($"Equals: {EvaluatePostfixTokens(CreatePostfixTokens(CreateInfixTokens(userInput)))}");
                    break;
                }
            }
        }
    }
}
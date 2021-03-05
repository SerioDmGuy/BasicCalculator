﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {    
        static bool IsEmpty<T>(Stack<T> stack)
        {
            if (stack.Count <= 0) return true;
            else return false;
        }
        
        // NOTE: Math operators are ordered by precedence here (lowest to highest)
        static readonly private string[] mathOperators = {"-", "+", "/", "*", "**", "^", "%", "mod"};
        
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
            // NOTE: Math operators are based on element precedence
            List<String> listOperators = new List<String>(mathOperators);
            int yourOperatorPrecedence = listOperators.IndexOf(yourOperator);
            int comparedOperatorPrecedence = listOperators.IndexOf(comparedOperator);
            
            // TODO BUG: Have all exponents treated as the same precedence
            // To fix an operator ordering bug with all exponents treated differently in order,
            // therefore making them the same.
            
            if (yourOperatorPrecedence > comparedOperatorPrecedence) return true;
            else return false;
        }

        static bool IsOnlyOneParenthesisPopped(bool openParenthesisPop, bool closeParenthesisPop)
        {       
            if (openParenthesisPop && !closeParenthesisPop)
            {     
                Console.WriteLine("ERROR: Expected closed parenthesis");
                return true;
            }

            else if (!openParenthesisPop && closeParenthesisPop)
            {
                Console.WriteLine("ERROR: Expected open parenthesis"); 
                return true;
            }
            else return false;
        }

        static bool IsParenthesis(string str)
        {
            if (str == "(" || str == ")") return true;
            else return false;
        }

        static bool IsNumber(string str)
        {
            if (Double.TryParse(str, out double foundNumber)) return true;
            else return false; 
        }

        static string PeekOrProvideElement(Stack<Match> stack, string provideElement)
        {
            if (IsEmpty<Match>(stack))
                return provideElement;
            else return stack.Peek().Value;
        }
        
        static void PopOnly(String thisElement, out bool isPopped, Stack<Match> stack)
        {
            // TODO: PopOnly should also return the element like regular pop can
            
            isPopped = false;
            
            if (IsEmpty<Match>(stack)) {}
            // TODO: Throw an empty exception if the stack itself is empty regular pop can

            else if (stack.Peek().Value == thisElement)
            {
                stack.Pop(); 
                isPopped = true;
            }
        }

        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            Regex infixTokens = new Regex(@"(?<FindSubtract>(?<=[)]|\d[%])[-])|(?<MatchNumbers>(?=[-]\d[.]|[-][.]\d|[.]\d|[-]\d|\d)(?!(?<=\d[.]|\d)[-](?=[.]?\d+))[-]?\d*[.]?\d*([%](?![.][\d]|[\d][.]|[-]|\d))?)|(?<MatchOperators>[*]{2}|[()\/*+%^-]|mod)|(?<IncludeInvalidTokens>.)");
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
                        EnqueuePostfixMathOperators(ref postfixTokens, ref operatorStack);
                }

                else if (IsMathOperator(token.Value))
                {   
                    if (!IsMathOperatorHigherPrecedence(token.Value, PeekOrProvideElement(operatorStack, token.Value)))
                        EnqueuePostfixMathOperators(ref postfixTokens, ref operatorStack);
                    operatorStack.Push(token);
                }
                else 
                { 
                    Console.Write("ERROR: Invalid infix token or tokens detected during postfix token creation ");
                    Console.WriteLine("make sure your infix expression is supported by Basic Calculator");
                    return null;
                }

                if (postfixTokens == null)
                {
                    Console.WriteLine("ERROR: Postfix token creation has been terminated."); 
                    return postfixTokens;
                }
            }
            
            EnqueuePostfixMathOperators(ref postfixTokens, ref operatorStack);

            if (!IsEmpty(operatorStack)) 
            {   
                Console.Write("ERROR: Leftover operator tokens within the operator stack aren't enqueued.");
                return null;
            }

            return postfixTokens;
        }

        static Queue<Match> EnqueuePostfixMathOperators(ref Queue<Match> yourQueue, ref Stack<Match> infixOperators)
        {   
            PopOnly(")", out bool isCloseParenthesisPop, infixOperators);
            
            while (!IsEmpty(infixOperators))
            {          
                if (IsParenthesis(infixOperators.Peek().Value))
                    break;
                else if (IsMathOperator(infixOperators.Peek().Value))
                    yourQueue.Enqueue(infixOperators.Pop());
                else 
                    {Console.WriteLine("ERROR: Detected an non-operator token that can't be enqueued."); break;}
            }
            
            if (isCloseParenthesisPop)
            { 
                PopOnly("(", out bool isOpenParenthesisPop, infixOperators);
                
                if (IsOnlyOneParenthesisPopped(isOpenParenthesisPop, isCloseParenthesisPop))
                    { 
                        Console.Write("ERROR: Open parenthesis missing from parenthesis pair, ");
                        Console.WriteLine("please ensure your parentheses pairs are correct and try again.");
                        return yourQueue = null; 
                    }
            }

            // TODO: If it's detected a closed parenthesis on it's own report an error.

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
                    numberStack.Push(EvaluateMathOperator(ref numberStack, postfixTokens.Dequeue().Value));
                
                else 
                { 
                    Console.Write($"ERROR: Detected unsupported token '{postfixTokens.Peek()}' can't be processed ");
                    Console.WriteLine("(Numbers or postfix tokens only) for Postfix Token Evaluator.");
                    return 0.0;
                }
            }            
            return GetResults(numberStack);
        }

        static double EvaluateMathOperator(ref Stack<Double> numberStack, String yourOperator)
        {
            if(!IsMathOperator(yourOperator))
                {Console.WriteLine($"ERROR: Can't evaluate two numbers using a non-maths operator"); return 0.0d;}
            
            if (numberStack.Count < 2)
                {Console.WriteLine($"ERROR: Can't evaluate your math operator '{yourOperator}' without two numbers."); return 0.0;}
            
            double secondNumber = numberStack.Pop();
            double firstNumber = numberStack.Pop();

            switch (yourOperator)
            {   
                case "mod": return GetEuclideanModulo(firstNumber, secondNumber);
                case "%": return GetEuclideanModulo(firstNumber, secondNumber);
                case "^": return GetPowerOfNumber(firstNumber, secondNumber);
                case "**": return GetPowerOfNumber(firstNumber, secondNumber);
                case "*": return (firstNumber * secondNumber);
                case "/": return (firstNumber / secondNumber);
                case "+": return (firstNumber + secondNumber);
                case "-": return (firstNumber - secondNumber);
                default: Console.Write($"ERROR: Math Operator '{yourOperator}' cannot evaluate because it's not supported");
                Console.WriteLine("by this calculator."); return 0.0d;
            }
        }

        static double GetPercent(double number)
        => number / 100d;
        
        static double GetEuclideanModulo(double dividend, double divisor)
        {
            int remainder = (int) dividend % (int) divisor;
            
            if (remainder < 0)
                remainder = (divisor < 0) ? remainder - (int) divisor
                : remainder + (int) divisor;

            return remainder;
        }
        
        static double GetPowerOfNumber(double baseNumber, double powerNumber)
        {
            double results = baseNumber * 1;

            for (int powerCount = 1;
            powerCount < powerNumber; powerCount++)
               { results = results * baseNumber; }
            
            return results;
        }

        static double GetResults(Stack<Double> yourStack)
        {
            if (yourStack.Count <= 0)
                {Console.WriteLine("ERROR: Results requires a number."); return 0.0d;}
            
            else if (IsNumber(yourStack.Peek().ToString()))
                return yourStack.Pop();
            
            else {Console.WriteLine("ERROR: Results must be a number"); return 0.0d;}
        }

        static void Main()
        {   
            bool calculatorOn = true;
            
            while (calculatorOn)
            {
                string userInput = String.Concat(Console.ReadLine().ToLower().Split(" "));
                
                while (calculatorOn) 
                {
                    if (userInput == "exit")
                        {calculatorOn = false; break;}
                    
                    else if (userInput == "clear" || userInput == "cls")
                        {Console.Clear(); break;}
                    
                    else if (String.IsNullOrEmpty(userInput) || String.IsNullOrWhiteSpace(userInput))
                        {Console.WriteLine("ERROR: Your input must not be empty."); break;}
                    
                    Console.WriteLine($"Equals: {EvaluatePostfixTokens(CreatePostfixTokens(CreateInfixTokens(userInput)))}");
                    // TODO: Abstract this line above into it's own function for readability sake.  
                    break;
                }
            }
        }
    }
}
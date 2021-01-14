using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {    
        static private char[] PMDAS = {'-', '+', '/', '*', '(', ')'};
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
            
            foreach (Char pmdasOperator in PMDAS)
            {
                if (pmdasOperator == Convert.ToChar(yourOperator.Value))
                    return true;
            } 
            return false;
        }

        static bool IsOperatorHigherPrecedence(Match yourOperator, Match compareOperator)
        {            
            List<Char> listOperators = new List<Char>(PMDAS);
            int yourOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(yourOperator.Value));
            int compareOperatorPrecedence = listOperators.IndexOf(Convert.ToChar(compareOperator.Value));
            
            if (yourOperatorPrecedence == -1 || compareOperatorPrecedence == -1)
            {
                Console.WriteLine($"ERROR: Couldn't compare operator's precedence because the chosen operators isn't supported, make sure it's one of these supported operators instead: '{listOperators}'");
                return false;
            }

            if (yourOperatorPrecedence >= compareOperatorPrecedence) return true;
            else return false;
        }

        static Queue<Match> EnqueueOperatorTokensFromOperatorStackUntilParentheses(ref Queue<Match> tokenQueue, ref Stack<Match> operatorStack)
        {
            int operatorsForEnqueuingCount = 0; 
            foreach (Match token in operatorStack)
            {
                if (token.Value == "(" || token.Value == ")") 
                    break;
                else if (IsOperator(token)) 
                    operatorsForEnqueuingCount++;
                else {Console.WriteLine("ERROR: Detected an non-operator token that can't be enqueued, therefore the operator stack is invalid");}
            }

            for (; operatorsForEnqueuingCount >= 1; operatorsForEnqueuingCount--)
                    { tokenQueue.Enqueue(operatorStack.Pop()); }

            return tokenQueue;
        }

        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            infixExpression = String.Concat(infixExpression.Split(' '));
            
            Regex createInfixTokens = new Regex(@"(?<FindNumbers>(?!(?<=\d)[-](?=[.]?\d+))[-]?\d*[.]?\d+)|(?<FindOperators>[()\/*+-])|(?<IncludeInvalidTokens>.)");
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
                    if (operatorStack.Count <= 0)
                        {Console.WriteLine("ERROR: A Closed parenthesis cannot be alone within operator stack"); return null;}
                    
                    else { EnqueueOperatorTokensFromOperatorStackUntilParentheses(ref postfixTokens, ref operatorStack);

                        if (operatorStack.Count >= 1)
                        {
                            if (operatorStack.Peek().Value == "(")
                                operatorStack.Pop();
                            else {Console.WriteLine("ERROR: All parenthesis pairs must be valid."); return null;}
                        } 
                        else {Console.WriteLine("ERROR: After enqueuing operators an open parenthesis token was to be expected within postfix Operator Stack"); return null;}
                    }
                }

                else if (IsOperator(token))
                {   
                    if (operatorStack.Count == 0)
                        operatorStack.Push(token);
                    
                    else if (IsOperatorHigherPrecedence(token, operatorStack.Peek())) 
                        operatorStack.Push(token);

                    else if (!IsOperatorHigherPrecedence(token, operatorStack.Peek()))
                    { 
                        if (operatorStack.Peek().Value == "(")
                            operatorStack.Push(token);
                        
                        else { EnqueueOperatorTokensFromOperatorStackUntilParentheses(ref postfixTokens, ref operatorStack);
                            operatorStack.Push(token); }
                    }   
                }
                
                else { Console.WriteLine("ERROR: Invalid infix token or tokens detected, make sure your infix expression is supported by Basic Calculator"); return null; }
            }
    
                EnqueueOperatorTokensFromOperatorStackUntilParentheses(ref postfixTokens, ref operatorStack);
                if (operatorStack.Count >= 1)
                {
                    Console.WriteLine("ERROR: Left over elements within the Operator Stack, should've been popped out during postfix token enqueueing.");
                    Console.WriteLine("Please ensure your infix expression/tokens are correct and supported (using this Basic Calculator) for successful conversion"); 
                    return null; 
                }
                return postfixTokens;
        }

        static Double EvaluatePostfixTokens(Queue<Match> postfixTokens)
        {
            Stack<Double> numberStack = new Stack<Double>();
            
            if (postfixTokens == null)
                { Console.WriteLine("ERROR: Postfix tokens are needed from your infix expression/tokens as you can't evaluate null."); return 0.0; }

            while (postfixTokens.Count > 0)
            {
                if (IsNumber(postfixTokens.Peek().Value))
                    numberStack.Push(Convert.ToDouble(postfixTokens.Dequeue().Value));

                else if (IsOperator(postfixTokens.Peek()))
                {
                    string yourOperator = postfixTokens.Dequeue().Value;

                    if (numberStack.Count < 2)
                    { Console.WriteLine($"ERROR: Detected Operator '{yourOperator}' can't complete it's evaluation without two numbers, therefore evaluation process has been terminated."); 
                    return 0.0; }
                    
                    double secondNumber = numberStack.Pop(), firstNumber = numberStack.Pop();

                    switch (yourOperator)
                    {   
                        case "*": numberStack.Push(firstNumber * secondNumber); break;
                        case "/": numberStack.Push(firstNumber / secondNumber); break;
                        case "+": numberStack.Push(firstNumber + secondNumber); break;
                        case "-": numberStack.Push(firstNumber - secondNumber); break;
                        default: Console.WriteLine($"ERROR: Operator token '{yourOperator}' cannot evaluate because it's not supported by Postfix Token Evaluator."); return 0.0;
                    } 
                } 
                else {Console.WriteLine($"ERROR: Detected unsupported token '{postfixTokens.Peek().Value}' can't be processed (Numbers or operator tokens only) for Postfix Token Evaluator."); return 0.0;}
            }

            if (numberStack.Count <= 0)
                {Console.WriteLine("ERROR: A number token generated via postfix evaluation is required for writing your results."); return 0.0;}
            
            double results = numberStack.Pop();

            if (numberStack.Count > 1)
                {Console.WriteLine($"ERROR:'{numberStack.Count}' more number tokens have been detected as only one number token can represent results."); return 0.0; }
            
            return results; 
        }

        static void Main()
        {   
            bool calculatorOn = true;

            while (calculatorOn)
            {
                string userInput = Console.ReadLine();
                
                if (userInput == "exit") 
                {calculatorOn = false; break;}
                
                while (calculatorOn) 
                {
                    if (userInput == "clear" || userInput == "cls")
                        {Console.Clear(); break;}
                    
                    else if (String.IsNullOrEmpty(userInput) || String.IsNullOrWhiteSpace(userInput))
                        {Console.WriteLine("ERROR: Your input must not be empty."); break;}
                    
                    Console.WriteLine(EvaluatePostfixTokens(CreatePostfixTokens(CreateInfixTokens(userInput))));
                    break;
                }
            }
        }
    }
}
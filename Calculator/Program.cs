using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator // TODO: Change directory from "Calculator" to "calculator". Double check before attempting todo.
{
    class Program
    {    
        class MathOperator
        {
            public MathOperator(string name, string[] symbols, int precedence, string associativity)
            {
                this.name = name;
                this.symbols = symbols;
                this.precedence = precedence;
                this.associativity = associativity;
            }
            public string name {get; set;}

            // Operators can have different symbols repreasenting the same operator.
            // think of them as alternatives such as "**" and "^" that represents power. 
            public string[] symbols {get; set;}
            public int precedence {get; set;}
            public string associativity {get; set;}
            public static readonly MathOperator Empty = new MathOperator("", new string[] {""}, -30, "");
        }

        // For each new math operator: name - symbols - precedence - associativity
        static MathOperator subtraction = new MathOperator("subtraction", new string[] {"-"}, 0, "left to right");
        static MathOperator addition = new MathOperator("addition", new string[] {"+"}, 0, "left to right");
        static MathOperator division = new MathOperator("division", new string[] {"/"}, 1, "left to right");
        static MathOperator multiplication = new MathOperator("multiplication", new string[] {"*"}, 1, "left to right");
        static MathOperator modulo = new MathOperator("modulo", new string[] {"%", "mod", "modulo", "modulus"}, 1, "left to right");
        static MathOperator power = new MathOperator("power", new string[] {"^", "**"}, 2, "right to left");
        static MathOperator[] mathOperators = {subtraction, addition, division, multiplication, modulo, power};

        public class Yard<T>
        {     
            public Queue<T> queue = new Queue<T>();
            public Stack<T> stack = new Stack<T>();
        }

        static MathOperator GetMathOperator(String str)
        {
            foreach (MathOperator mathOperator in mathOperators)
            {
                foreach (String symbol in mathOperator.symbols)
                {
                    if (str.ToLower() == symbol)
                        return mathOperator;
                }
            }
            return MathOperator.Empty;
        }

        static bool IsMathOperator(string str) => GetMathOperator(str) != MathOperator.Empty;

        static bool IsMathOperatorPrecedenceHigher(string yourOp, string compareOp) => GetMathOperator(yourOp).precedence > GetMathOperator(compareOp).precedence;

        static bool IsMathOperatorPrecedenceLower(string yourOp, string compareOp) => GetMathOperator(yourOp).precedence < GetMathOperator(compareOp).precedence;

        static bool IsNumber(string str) => str == "NaN" ? false : (Double.TryParse(str, out double num) || IsPercentNumber(str)) ? true : false;

        static bool IsPercentNumber(string str)
        {
            if (str.Length >= 2)
                if (str[str.Length-1] == '%' && str != "NaN%")
                    if (Double.TryParse(str.Remove(str.Length-1), out double num))
                        return true;
            return false;
        }
        
        static double PercentToNumber(string percentNumber)
        {
            if (IsPercentNumber(percentNumber))
                return Double.Parse(percentNumber.Remove(percentNumber.Length-1)) / 100;
            else Console.WriteLine("ERROR: Required a Percent number for number conversion.");
            return 0;
        }

        static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            // Cleans up infix expression by removing whitespaces .etc
            infixExpression = Regex.Replace(string.Concat(infixExpression.ToLower().Trim().Split(" ")), "((?<=\\d\\.?)\\()|(?:(\\))(?=\\.?\\d))|(\\)\\()", delegate(Match token)
            {
                // Places asterisks between numbers/parenthesis
                switch (token.Value)
                {
                    case "(": return "*(";
                    case ")": return ")*";
                    case ")(": return ")*(";
                }
                return token.Value;
            });

            // Splits infix tokens from a infix expression via regex
            MatchCollection infixTokens = Regex.Matches(infixExpression, @"(?<MatchNumbers>(?=[-]\d[.]|[-][.]\d|[.]\d|[-]\d|\d)(?!(?<=[)]|\d[.]|\d)[-](?=[.]?\d+))[-]?\d*[.]?\d*([%](?![(]|[-][.][\d]|[.][\d]|[\d][.]|[-]\d|\d))?)|(?<MatchOperators>[*]{2}|[()\/*+%^-]|modulus|modulo|mod)|(?<MatchNonInfixTokens>\S)");
            
            foreach (Match token in infixTokens)
            {
                if (token.Groups[4].Name == "MatchNonInfixTokens")
                {
                    if (token.Groups[4].Success)
                    {
                        Console.WriteLine("ERROR: Invalid 'infix' token/s detected during infix token creation");
                        return Regex.Matches("", "");
                    }
                }
            }
            return infixTokens;
        }

        static Queue<String> InfixToPostfixTokens(MatchCollection infixTokens)
        {   
            // Converts infix tokens using the "Shunting yard algorithm".
            // Essentially enqueuing numbers becomes a postfix token.
            // Repositioning infix math operators are done using a stack.

            Yard<String> postfixYard = new Yard<String>();
            
            foreach (Match token in infixTokens)
            {
                // Each number becomes a postfix token
                if (IsNumber(token.Value)) 
                    postfixYard.queue.Enqueue(IsPercentNumber(token.Value) ? $"{PercentToNumber(token.Value)}" : token.Value);
                
                // Each parenthesis joins the stack
                else if (token.Value == "(" | token.Value == ")")
                {
                    postfixYard.stack.Push(token.Value);
                    
                    if (")" == token.Value) 
                    {   
                        bool isCloseParenthesisPop = postfixYard.stack.TryPeek(out String topElement) && ")" == topElement ? postfixYard.stack.TryPop(out String _) : false;
                        EnqueueMathOperators(ref postfixYard.queue, ref postfixYard.stack);
                        bool isOpenParenthesisPop = postfixYard.stack.TryPeek(out String top) && "(" == top ? postfixYard.stack.TryPop(out String _) : false;

                        if (!isOpenParenthesisPop && !isCloseParenthesisPop)
                            Console.WriteLine("ERROR: Parenthesis must be in a pair ()");
                    }
                }
                
                else if (IsMathOperator(token.Value))
                {
                    if (IsMathOperatorPrecedenceHigher(token.Value, postfixYard.stack.TryPeek(out String topOp) ? topOp : "")
                    || GetMathOperator(token.Value).precedence == GetMathOperator(topOp).precedence
                    && GetMathOperator(topOp).associativity == "right to left")
                        postfixYard.stack.Push(token.Value);
                    
                    else if (IsMathOperatorPrecedenceLower(token.Value, topOp) 
                    || GetMathOperator(token.Value).precedence == GetMathOperator(topOp).precedence
                    && GetMathOperator(topOp).associativity == "left to right")
                    {
                        for (int i = 0; i <= postfixYard.stack.Count;)
                        {
                            if (postfixYard.stack.Count == 0 || IsMathOperatorPrecedenceHigher(token.Value, postfixYard.stack.Peek())) {
                                postfixYard.stack.Push(token.Value); break; }
                            
                            else postfixYard.queue.Enqueue(postfixYard.stack.Pop());
                        }
                    }
                }

                else // Not an infix token
                { 
                    Console.WriteLine("ERROR: Invalid 'infix' token/s detected during infix to postfix conversion.");
                    return postfixYard.queue;
                }
            }
            // At the end all math operators get enqueued from the stack
            EnqueueMathOperators(ref postfixYard.queue, ref postfixYard.stack);

            // Checks for leftover math operators within the stack
            if (postfixYard.stack.Count >= 1)
                Console.WriteLine("ERROR: Leftover Operators detected within infix to postfix stack aren't enqueued.");

            return postfixYard.queue;
        }

        static Queue<String> EnqueueMathOperators(ref Queue<String> queue, ref Stack<String> operators)
        {   
            while (operators.Count >= 1)
            {          
                if (IsMathOperator(operators.Peek()))
                    queue.Enqueue(operators.Pop());
                else break;
            }
            return queue;
        } 

        static Double EvaluatePostfixTokens(Queue<String> postfixTokens)
        {
            // Evaluates postfix-tokens using an algorithm called Postfix Stack Evaluator
            Stack<Double> numberStack = new Stack<Double>();
            foreach (String token in postfixTokens)
            {
                if (IsNumber(token))
                    numberStack.Push(Convert.ToDouble(token));
                else if (IsMathOperator(token))
                    numberStack.Push(EvaluateMathOperator(token, ref numberStack)); 
                else 
                {
                    Console.WriteLine($"ERROR: Can't evaluate a non-postfix token '{token}'");
                    break;
                }
            }           
            return numberStack.TryPeek(out Double number) ? number : 0;
        }

        static double EvaluateMathOperator(String mathOperator, ref Stack<Double> yourNumbers)
        {
            if (yourNumbers.Count < 2) 
            {
                Console.WriteLine($"ERROR: A math operator '{mathOperator}' requires two numbers."); 
                return 0d;
            }
            
            double secondNumber = yourNumbers.Pop();
            double firstNumber = yourNumbers.Pop();
            switch (GetMathOperator(mathOperator).name)
            {   
                case "power": return Math.Pow(firstNumber, secondNumber);
                case "modulo": return GetModulo(firstNumber, secondNumber);
                case "multiplication": return (firstNumber * secondNumber);
                case "division": return GetDivision(firstNumber, secondNumber);
                case "addition": return (firstNumber + secondNumber);
                case "subtraction": return (firstNumber - secondNumber);
                default: Console.Write($"ERROR: Cannot evaluate unsupported math operator '{mathOperator}'"); return 0d;
            }
        }

        static double GetDivision(double dividend, double divisor)
        {
            if (divisor == 0) 
            {
                Console.WriteLine("ERROR: You cannot divide by zero!");
                return double.NaN;
            }
            else return dividend / divisor;
        }

        static double GetModulo(double dividend, double divisor)
        {
            if (divisor == 0)
            {
                Console.WriteLine("ERROR: You cannot divide by zero!");
                return double.NaN;
            }
            else return dividend % divisor;
        }

        static void Main(string[] args)
        {   
            bool calculatorOn = true;

            while (calculatorOn)
            {   
                Console.WriteLine("Please write your infix expression for calculation");
                Console.Write(">"); 
                string userInput = Console.ReadLine().ToLower().Trim();

                if (userInput == "exit") { 
                    calculatorOn = false; break; }
                else
                {
                    Queue<String> postfixTokens = InfixToPostfixTokens(CreateInfixTokens(userInput));
                    Double results = EvaluatePostfixTokens(postfixTokens);

                    Console.Write("Postfix: ");
                    Array.ForEach(postfixTokens.ToArray(), token => Console.Write($"{token} "));
                    Console.WriteLine("\nResults: {0}\n", results);
                }
            }
        }
    }
}
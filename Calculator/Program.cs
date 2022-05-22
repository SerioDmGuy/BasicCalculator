using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator // TODO: Double Check all member conventions. (pascal and camel casings .etc)
{
    public class Program
    {    
        public class MathOperator
        {
            public MathOperator(string name, string[] symbols, int precedence, string associativity)
            {
                this.name = name;
                this.symbols = symbols;
                this.precedence = precedence;
                this.associativity = associativity;
            }
            public string name {get; set;}
            // Operators can have different symbols representing the same operator.
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
        static MathOperator modulo = new MathOperator("modulo", new string[] {"%", "mod", "modulo"}, 1, "left to right");
        static MathOperator power = new MathOperator("power", new string[] {"^", "**", "pow", "power"}, 2, "right to left");
        public static MathOperator[] mathOperators = {subtraction, addition, division, multiplication, modulo, power};

        public class Yard<T>
        {     
            public Queue<T> queue = new Queue<T>();
            public Stack<T> stack = new Stack<T>();
        }

        public static MathOperator GetMathOperator(String str)
        {
            if (str == null) return MathOperator.Empty;
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

        public static bool IsMathOperator(string str) => GetMathOperator(str) != MathOperator.Empty;

        public static bool IsNumber(string str) => str == "NaN" ? false : (Double.TryParse(str, out double num) || IsPercentNumber(str)) ? true : false;

        public static bool IsPercentNumber(string str)
        {
            if (str.Length >= 2)
                if (str[str.Length-1] == '%' && str != "NaN%")
                    if (Double.TryParse(str.Remove(str.Length-1), out double num))
                        return true;
            return false;
        }
        
        public static double PercentToNumber(string percentNumber)
        {
            if (IsPercentNumber(percentNumber))
                return Double.Parse(percentNumber.Remove(percentNumber.Length-1)) / 100;
            else Console.WriteLine("ERROR: PercentToNumber() requires a percent number.");
            return 0;
        }

        public static MatchCollection CreateInfixTokens(string infixExpression)
        {   
            if (infixExpression == null) return Regex.Matches("boo", @"foo");

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
            MatchCollection infixTokens = Regex.Matches(infixExpression, @"(?<MatchNumbers>(?<![.](?!\d))(?!\d[.]{2})(?=[-][.]\d(?![.])|[-]\d|[.]\d(?![.])|\d)(?!(?<=[)]|\d[.]|\d)[-](?=[.]?\d+))[-]?\d*[.]?\d*([%](?![(]|[-][.][\d]|[.][\d]|[\d][.]|[-]\d|\d))?|\d)|(?<MatchOperators>[*]{2}|[()\/*+%^-]|modulo|mod|power|pow)|(?<MatchNonInfixTokens>\S)");
             
            foreach (Match token in infixTokens)
            {
                if (token.Groups[4].Name == "MatchNonInfixTokens")
                {
                    if (token.Groups[4].Success)
                    {
                        Console.WriteLine("ERROR: Invalid 'infix' token/s detected during infix token creation");
                        return Regex.Matches("boo", @"foo");
                    }
                }
            }
            return infixTokens;
        }

        public static Queue<String> InfixToPostfixTokens(MatchCollection infixTokens)
        {   
            if (infixTokens == null) return new Queue<String>();
            
            // Converts infix tokens using the "Shunting yard algorithm".
            Yard<String> postfixYard = new Yard<String>();
            foreach (Match token in infixTokens)
            {
                // Enqueuing numbers becomes a postfix token.
                if (IsNumber(token.Value)) 
                    postfixYard.queue.Enqueue(IsPercentNumber(token.Value) ? $"{PercentToNumber(token.Value)}" : token.Value);
                
                // Each parenthesis joins the stack
                else if (token.Value == "(" | token.Value == ")")
                {
                    postfixYard.stack.Push(token.Value);
                    
                    if (")" == token.Value) 
                    {   
                        bool isCloseParenthesisPop = postfixYard.stack.TryPeek(out String topElement) && ")" == topElement ? postfixYard.stack.TryPop(out String _) : false;
                        EnqueueMathOperators(ref postfixYard, MathOperator.Empty.precedence);
                        bool isOpenParenthesisPop = postfixYard.stack.TryPeek(out String top) && "(" == top ? postfixYard.stack.TryPop(out String _) : false;

                        if (!isOpenParenthesisPop && !isCloseParenthesisPop)
                            Console.WriteLine("ERROR: Parenthesis must be in a pair ()");
                    }
                }
                
                // Repositioning math operators are done using a stack.
                else if (IsMathOperator(token.Value))
                {
                    MathOperator yourOperator = GetMathOperator(token.Value);
                    MathOperator stackOperator = postfixYard.stack.TryPeek(out string top) ? GetMathOperator(top) : MathOperator.Empty;
                    if (yourOperator.precedence > stackOperator.precedence)
                        postfixYard.stack.Push(token.Value);
                    else if (yourOperator.precedence < stackOperator.precedence)
                    {
                        EnqueueMathOperators(ref postfixYard, yourOperator.precedence-1);
                        postfixYard.stack.Push(token.Value);
                    }
                    else if (yourOperator.precedence == stackOperator.precedence && stackOperator.associativity == "left to right")
                    {
                        EnqueueMathOperators(ref postfixYard, yourOperator.precedence-1);
                        postfixYard.stack.Push(token.Value);
                    }
                    else if (yourOperator.precedence == stackOperator.precedence && stackOperator.associativity == "right to left")
                        postfixYard.stack.Push(token.Value);
                }

                else // Not an infix token
                { 
                    Console.WriteLine("ERROR: Invalid 'infix' token/s detected during infix to postfix conversion.");
                    return new Queue<String>();
                }
            }
            // At the end all math operators get enqueued from the stack
            EnqueueMathOperators(ref postfixYard, MathOperator.Empty.precedence);

            // Checks for leftover math operators within the stack
            if (postfixYard.stack.Count >= 1)
                Console.WriteLine("ERROR: Leftover Operators within InfixToPostfixTokens() stack aren't enqueued.");

            return postfixYard.queue;
        }

        public static void EnqueueMathOperators(ref Yard<String> yard, int stopAtPrecedence)
        {   
            while (yard.stack.Count >= 1)
            {          
                if (IsMathOperator(yard.stack.Peek()) && GetMathOperator(yard.stack.Peek()).precedence > stopAtPrecedence)
                    yard.queue.Enqueue(yard.stack.Pop());
                else break;
            }
        } 

        // Evaluates postfix-tokens using an algorithm called Postfix Stack Evaluator
        public static Double EvaluatePostfixTokens(Queue<String> postfixTokens)
        {
            if (postfixTokens == null) return 0d;
            
            Stack<Double> numberStack = new Stack<Double>();
            foreach (String token in postfixTokens)
            {
                if (IsNumber(token))
                    numberStack.Push(IsPercentNumber(token) ? PercentToNumber(token) : Convert.ToDouble(token));
                else if (IsMathOperator(token))
                    numberStack.Push(EvaluateMathOperator(token, ref numberStack)); 
                else {
                    Console.WriteLine($"ERROR: Can't evaluate a non-postfix token '{token}'"); return 0d;}
            }           
            return numberStack.TryPeek(out Double number) ? number : 0;
        }

        public static double EvaluateMathOperator(String mathOperator, ref Stack<Double> yourNumbers)
        {
            bool isSecondNumberPop = yourNumbers.TryPop(out double secondNumber);
            bool isFirstNumberPop = yourNumbers.TryPop(out double firstNumber);
            
            if (!isFirstNumberPop || !isSecondNumberPop)
            {
                Console.WriteLine($"ERROR: A math operator '{mathOperator}' requires two numbers.");
                return 0d;
            }

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
        
        public static double GetDivision(double dividend, double divisor)
        {
            if (divisor != 0) return dividend / divisor;
            else {Console.WriteLine("ERROR: You cannot divide by zero!"); return double.NaN;} 
        }

        public static double GetModulo(double dividend, double divisor)
        => (divisor == 0) ? 0 : (Math.Sign(dividend) == -1 && Math.Sign(divisor) == -1) && dividend < divisor && divisor >= -0.01 ? dividend - (dividend / divisor) * divisor : dividend - (int)Math.Floor((double)dividend / divisor) * divisor;        

        static void Main(string[] args)
        {   
            bool calculatorOn = true;

            while (calculatorOn)
            {   
                Console.Write($"Math Operators: "); 
                Array.ForEach(mathOperators, op => Console.Write($"{op.symbols[0]} "));
                Console.WriteLine("\nPlease write your infix expression for calculation");
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
                    Console.WriteLine("\nResults: {0}\n", Double.IsPositiveInfinity(results) ? "Infinity" : Double.IsNegativeInfinity(results) ? "-Infinity" : results);
                }
            }
        }
    }
}
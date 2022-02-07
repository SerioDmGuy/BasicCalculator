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
            public static readonly MathOperator Empty = new MathOperator("", new string[] {""}, 0, "");
        }

        // For each new math operator: name - symbols - precedence - associativity
        static MathOperator subtraction = new MathOperator("subtraction", new string[] {"-"}, 0, "left to right");
        static MathOperator addition = new MathOperator("addition", new string[] {"+"}, 0, "left to right");
        static MathOperator division = new MathOperator("division", new string[] {"/"}, 1, "left to right");
        static MathOperator multiplication = new MathOperator("multiplication", new string[] {"*"}, 1, "left to right");
        static MathOperator modulo = new MathOperator("modulo", new string[] {"%", "mod", "modulo", "modulus"}, 1, "left to right");
        static MathOperator power = new MathOperator("power", new string[] {"^", "**"}, 2, "right to left");
        static MathOperator[] mathOperators = {subtraction, addition, division, multiplication, modulo, power};

        struct Yard<T>
        {
            public Queue<T> queue;
            public Stack<T> stack;            
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

        static bool IsMathOperator(string str)
        {   
            if (GetMathOperator(str) != MathOperator.Empty)
                return true;
            return false;
        }

        static bool IsMathOperatorPrecedenceHigher(string yourOperator, string compareOperator)
        {            
            if (IsMathOperator(yourOperator) && IsMathOperator(compareOperator))
            {
                if (GetMathOperator(yourOperator).precedence > GetMathOperator(compareOperator).precedence)
                    return true;
            }
            return false;
        }

        static bool IsNumber(string str)
        {
            if (str == "NaN") // Not a Number
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

        static bool IsParenthesisPairPopped(bool isOpenParenthesisPop, bool isCloseParenthesisPop)
        {       
            if (isOpenParenthesisPop && isCloseParenthesisPop)
                return true;
            
            return false;
        }

        static bool IsEmpty<T>(Stack<T> stack)
        {
            if (stack.Count <= 0) return true;
            return false;
        }

        static string PeekOrProvideElement(Stack<Match> stack, string element)
        {
            //TODO: Check both parameters for null argument exception
            
            if (IsEmpty<Match>(stack))
                return element;
            
            return stack.Peek().Value;
        }
        
        static void PopOnly(String thisElement, ref Stack<Match> stack, out bool isPopped)
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
            // By spilting up the infix expression into tokens using regex
            // matching either numbers, parentheses or supported math operators.
            
            // TODO: Split Regex into their own strings rather than a massive one liner
            // TODO: Check for invalid characters!!!
            Regex infixTokens = new Regex(@"(?<FindNumbers>(?=[-]\d[.]|[-][.]\d|[.]\d|[-]\d|\d)(?!(?<=\d[.]|\d)[-](?=[.]?\d+))[-]?\d*[.]?\d*([%](?![.][\d]|[\d][.]|[-]|\d))?)|(?<FindOperators>[*]{2}|[()\/*+%^-]|mod)");
            return infixTokens.Matches(infixExpression);
        }

        static Queue<Match> InfixToPostfixTokens(MatchCollection infixTokens)
        {   
            // Converts infix tokens using the "Shunting yard algorithm".
            // Essentially enqueuing numbers becomes a postfix token.
            // Repositioning infix math operators are done using a stack.

            Yard<Match> postfixYard = new Yard<Match>();
            
            foreach (Match token in infixTokens)
            {
                // Each number becomes a postfix token
                if (IsNumber(token.Value)) 
                    postfixYard.queue.Enqueue(token);
                
                // Each parenthesis joins the stack
                else if (IsParenthesis(token.Value))
                {
                    postfixYard.stack.Push(token);
                    
                    if (")" == token.Value) 
                        EnqueueMathOperatorsFromParenthesisPair(ref postfixYard);
                }
                
                // --------------------------------------------------------
                // TODO: Enqueue Math Operators checking 4 rules in mind.
                // --------------------------------------------------------

                // You can't process non-infix tokens so therefore we terminate
                else 
                { 
                    Console.WriteLine("ERROR: Invalid 'infix' token/s detected during infix to postfix.");
                    return postfixYard.queue;
                }
            }
            
            // At the end all math operators get enqueued from the stack 
            EnqueueMathOperators(ref postfixYard.queue, ref postfixYard.stack);

            // Checks for leftover math operators within the stack
            if (!IsEmpty(postfixYard.stack)) 
                Console.WriteLine("ERROR: Leftover infix to postfix stack math operators aren't enqueued.");

            return postfixYard.queue;
        }

        static Yard<Match> PushMathOperatorToPostfixStack(Match mathOperator, ref Yard<Match> postfixYard)
        {
            if (!IsMathOperator(mathOperator.Value))
                {
                    Console.WriteLine("ERROR: Must provide a Math Operator to push Postfix's Stack"); 
                    return postfixYard; 
                }
            
            // Determines all rules for sorting an math operator below
            if (IsEmpty(postfixYard.stack))
                postfixYard.stack.Push(mathOperator);

            // TODO: if     math operator has higher precedence compared to operator at top of stack.
            // do   push math operator into stack

            // TODO: If operator same precedence as operator on top of stack, check for associativity.
            
        }

        static Yard<Match> EnqueueMathOperatorsFromParenthesisPair(ref Yard<Match> yourYard)
        {
            PopOnly(")", ref yourYard.stack, out bool isCloseParenthesisPop);
            EnqueueMathOperators(ref yourYard.queue, ref yourYard.stack);
            PopOnly("(", ref yourYard.stack, out bool isOpenParenthesisPop);

            if (!IsParenthesisPairPopped(isOpenParenthesisPop, isCloseParenthesisPop))
                Console.WriteLine("ERROR: Parenthesis must be in a pair ()"); 
            
            return yourYard;
        }

        static Queue<Match> EnqueueMathOperators(ref Queue<Match> yourQueue, ref Stack<Match> mathOperators)
        {   
            while (!IsEmpty(mathOperators))
            {          
                if (IsMathOperator(mathOperators.Peek().Value))
                    yourQueue.Enqueue(mathOperators.Pop());
                else break;
            }

            return yourQueue;
        } 

        static Double EvaluatePostfixTokens(Queue<Match> postfixTokens)
        {
            Stack<Double> numberStack = new Stack<Double>();
            
            foreach (Match token in postfixTokens)
            {
                if (IsNumber(token.Value))
                    numberStack.Push(Convert.ToDouble(token.Value));
                
                else if (IsMathOperator(token.Value))
                    numberStack.Push(EvaluateMathOperator(token.Value, ref numberStack)); 

                else 
                    { Console.WriteLine($"ERROR: Can't evaluate a non-postfix token '{token.Value}'"); break; }
            }           
            
            return GetNumber(numberStack);
        }

        static double EvaluateMathOperator(String mathOperator, ref Stack<Double> yourNumbers)
        {
            if (yourNumbers.Count < 2)
            {
                // TODO: This if statement isn't necessary
                Console.WriteLine($"ERROR: Can't evaluate a maths operator without at least two numbers."); 
                return 0;
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
                case "/": return GetEuclideanDivide(firstNumber, secondNumber);
                case "+": return (firstNumber + secondNumber);
                case "-": return (firstNumber - secondNumber);
                default: Console.Write($"ERROR: Cannot evaluate because it's not a supported math operator '{mathOperator}'"); return 0.0d;
            }
        }

        static double GetEuclideanDivide(double dividend, double divisor)
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
            // Returns the remainder of a Euclidean division (without factional parts)

            // TODO: Append and return the factional part
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

        // Gets a number from a stack and returns it.
        // TODO: Rewrite function identifier to SetResults?
        // TODO: If found an element, Parse element ensuring it's an number.
        static double GetNumber(Stack<Double> yourStack) => (yourStack.Count >= 1) ? yourStack.Pop() : 0;

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
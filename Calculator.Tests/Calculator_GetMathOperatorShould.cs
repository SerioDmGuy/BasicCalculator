using System;
using NUnit.Framework;

namespace Calculator.Tests
{
    public class Calculator_GetMathOperatorShould
    {
        [Test]
        public void GetMathOperator_MathOperators_ReturnsMathOperators()
        {
            foreach (Program.MathOperator mathOperator in Program.MathOperators)            
                foreach (String symbol in mathOperator.Symbols)
                    Assert.AreEqual(mathOperator.Name, Program.GetMathOperator(symbol).Name);
        }
        
        [TestCase("   ")]
        [TestCase("askjoaskjoas")]
        [TestCase("\\\\")]
        [TestCase(".. . . .")]
        [TestCase("$^£&£*£")]
        public void GetMathOperator_InvalidCharacters_ReturnsEmpty(string invalidCharSeq)
        => Assert.AreEqual(Program.GetMathOperator(invalidCharSeq), Program.MathOperator.Empty);
        
        [Test]
        public void GetMathOperator_NullExpression_ReturnsEmpty()
        => Assert.AreEqual(Program.GetMathOperator(null), Program.MathOperator.Empty);
    }
}
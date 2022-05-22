using System;
using NUnit.Framework;

namespace Calculator.Tests
{
    public class Calculator_GetMathOperatorShould
    {
        [Test]
        public void GetMathOperator_MathOperators_ReturnsMathOperators()
        {
            foreach (Program.MathOperator mathOperator in Program.mathOperators)            
                foreach (String symbol in mathOperator.symbols)
                    Assert.AreEqual(mathOperator.name, Program.GetMathOperator(symbol).name);
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
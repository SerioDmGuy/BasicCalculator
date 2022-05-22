using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Calculator.Tests
{
    public class Calculator_InfixToPostfixTokensShould
    {
        [SetUp] public void Setup() {}
        
        [TestCase("2--", new String[] {"2", "-", "-"})]
        [TestCase("--2", new String[] {"-2", "-"})]
        [TestCase("2++", new String[] {"2", "+", "+"})]
        [TestCase("+2+", new String[] {"2", "+", "+"})]
        [TestCase("2//", new String[] {"2", "/", "/"})]
        [TestCase("2%%", new String [] {"0.02", "%"})]
        [TestCase("2%%-15", new String [] {"0.02", "-15", "%"})]
        [TestCase("2%mod-29", new String [] {"0.02", "-29", "mod"})]
        [TestCase("2modmod", new String[] {"2", "mod", "mod"})]
        [TestCase("2modulomodulo", new String[] {"2", "modulo", "modulo"})]
        [TestCase("2**", new String[] {"2", "**"})]
        [TestCase("2^^", new String[] {"2", "^", "^"})]
        public void InfixToPostfixTokens_TokenSequence_ReturnsExpectedTokens(string tokenSeq, String[] expectedTokens)
        => Assert.AreEqual(Program.InfixToPostfixTokens(Program.CreateInfixTokens(tokenSeq)), expectedTokens);
        
        [TestCase("2.-2", new String[] {"2.", "2", "-"})]
        [TestCase("2+2", new String[] {"2", "2", "+"})]
        [TestCase("5-2", new String[] {"5", "2", "-"})]
        [TestCase("3/5", new String[] {"3", "5", "/"})]
        [TestCase("4*6", new String[] {"4", "6", "*"})]
        [TestCase("9%2", new String[] {"9", "2", "%"})]
        [TestCase("3%.2%", new String[] {"3", "0.002", "%"})]
        [TestCase("-4.%-2.", new String[] {"-4.", "-2.", "%"})]
        [TestCase("2mod2", new String[] {"2", "2", "mod"})]
        [TestCase("5modulo2", new String[] {"5", "2", "modulo"})]
        [TestCase("3**4", new String[] {"3", "4", "**"})]
        [TestCase("8^3", new String[] {"8", "3", "^"})]
        [TestCase("3^4^6", new String[] {"3", "4", "6", "^", "^"})]
        [TestCase("8-2+(3*4)/2^2", new String[] {"8", "2", "-", "3", "4", "*", "2", "2", "^", "/", "+"})]
        [TestCase("-2.%%-.2%2", new String[] {"-0.02", "-.2", "%", "2", "%"})]
        [TestCase("5.+-2%^8", new String[] {"5.", "-0.02", "8", "^", "+",})]
        public void InfixToPostfixTokens_InfixExpressions_ReturnsExpectedTokens(string infixExp, String[] expectedTokens)
        => Assert.AreEqual(Program.InfixToPostfixTokens(Program.CreateInfixTokens(infixExp)), expectedTokens);

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("2+2!")]
        [TestCase("8-2+(3..4)/2^2")]
        [TestCase("-2..2")]
        [TestCase("-.2.2")]
        public void InfixToPostfixTokens_InvalidInfixCharacters_ReturnsEmpty(string invalidInfixExp)
        => Assert.AreEqual(Program.InfixToPostfixTokens(Program.CreateInfixTokens(invalidInfixExp)), new Queue<String>());
        
        [Test]
        public void InfixToPostfixTokens_EmptyMatchCollection_ReturnsEmpty()
        => Assert.AreEqual(Program.InfixToPostfixTokens(Regex.Matches("boo", @"foo")), new Queue<String>());
        
        [Test]
        public void InfixToPostfixTokens_NullExpression_ReturnsEmpty()
        => Assert.AreEqual(Program.InfixToPostfixTokens(null), new Queue<String>());
    }
}
using System;
using NUnit.Framework;

namespace Calculator.Tests
{
    public class Calculator_CreateInfixTokensShould
    {
        [SetUp] public void Setup() {}
        
        [TestCase(" 2    2     ", new String[] {"22"})]
        [TestCase("   2. 2    ", new String[] {"2.2"})]
        [TestCase("   2 . 2    ", new String[] {"2.2"})]
        [TestCase("0.0", new String[] {"0.0"})]
        [TestCase("0", new String[] {"0"})]
        [TestCase("1", new String[] {"1"})]
        [TestCase("2", new String[] {"2"})]
        [TestCase("3", new String[] {"3"})]
        [TestCase("4", new String[] {"4"})]
        [TestCase("5", new String[] {"5"})]
        [TestCase("6", new String[] {"6"})]
        [TestCase("7", new String[] {"7"})]
        [TestCase("8", new String[] {"8"})]
        [TestCase("9", new String[] {"9"})]
        [TestCase("0.0%", new String[] {"0.0%"})]
        [TestCase("0%", new String[] {"0%"})]
        [TestCase("1%", new String[] {"1%"})]
        [TestCase("2%", new String[] {"2%"})]
        [TestCase("3%", new String[] {"3%"})]
        [TestCase("4%", new String[] {"4%"})]
        [TestCase("5%", new String[] {"5%"})]
        [TestCase("6%", new String[] {"6%"})]
        [TestCase("7%", new String[] {"7%"})]
        [TestCase("8%", new String[] {"8%"})]
        [TestCase("9%", new String[] {"9%"})]
        [TestCase("-0.0", new String[] {"-0.0"})]
        [TestCase("-0", new String[] {"-0"})]
        [TestCase("-1", new String[] {"-1"})]
        [TestCase("-2", new String[] {"-2"})]
        [TestCase("-3", new String[] {"-3"})]
        [TestCase("-4", new String[] {"-4"})]
        [TestCase("-5", new String[] {"-5"})]
        [TestCase("-6", new String[] {"-6"})]
        [TestCase("-7", new String[] {"-7"})]
        [TestCase("-8", new String[] {"-8"})]
        [TestCase("-9", new String[] {"-9"})]
        [TestCase("-0.0%", new String[] {"-0.0%"})]
        [TestCase("-0%", new String[] {"-0%"})]
        [TestCase("-1%", new String[] {"-1%"})]
        [TestCase("-2%", new String[] {"-2%"})]
        [TestCase("-3%", new String[] {"-3%"})]
        [TestCase("-4%", new String[] {"-4%"})]
        [TestCase("-5%", new String[] {"-5%"})]
        [TestCase("-6%", new String[] {"-6%"})]
        [TestCase("-7%", new String[] {"-7%"})]
        [TestCase("-8%", new String[] {"-8%"})]
        [TestCase("-9%", new String[] {"-9%"})]
        [TestCase("0.", new String[] {"0."})]
        [TestCase("1.", new String[] {"1."})]
        [TestCase("2.", new String[] {"2."})]
        [TestCase("3.", new String[] {"3."})]
        [TestCase("4.", new String[] {"4."})]
        [TestCase("5.", new String[] {"5."})]
        [TestCase("6.", new String[] {"6."})]
        [TestCase("7.", new String[] {"7."})]
        [TestCase("8.", new String[] {"8."})]
        [TestCase("9.", new String[] {"9."})]
        [TestCase(".0", new String[] {".0"})]
        [TestCase(".1", new String[] {".1"})]
        [TestCase(".2", new String[] {".2"})]
        [TestCase(".3", new String[] {".3"})]
        [TestCase(".4", new String[] {".4"})]
        [TestCase(".5", new String[] {".5"})]
        [TestCase(".6", new String[] {".6"})]
        [TestCase(".7", new String[] {".7"})]
        [TestCase(".8", new String[] {".8"})]
        [TestCase(".9", new String[] {".9"})]
        [TestCase("-0.", new String[] {"-0."})]
        [TestCase("-1.", new String[] {"-1."})]
        [TestCase("-2.", new String[] {"-2."})]
        [TestCase("-3.", new String[] {"-3."})]
        [TestCase("-4.", new String[] {"-4."})]
        [TestCase("-5.", new String[] {"-5."})]
        [TestCase("-6.", new String[] {"-6."})]
        [TestCase("-7.", new String[] {"-7."})]
        [TestCase("-8.", new String[] {"-8."})]
        [TestCase("-9.", new String[] {"-9."})]
        [TestCase("-.0", new String[] {"-.0"})]
        [TestCase("-.1", new String[] {"-.1"})]
        [TestCase("-.2", new String[] {"-.2"})]
        [TestCase("-.3", new String[] {"-.3"})]
        [TestCase("-.4", new String[] {"-.4"})]
        [TestCase("-.5", new String[] {"-.5"})]
        [TestCase("-.6", new String[] {"-.6"})]
        [TestCase("-.7", new String[] {"-.7"})]
        [TestCase("-.8", new String[] {"-.8"})]
        [TestCase("-.9", new String[] {"-.9"})]
        [TestCase("-0.01", new String[] {"-0.01"})]
        [TestCase("-0.02", new String[] {"-0.02"})]
        [TestCase("-0.03", new String[] {"-0.03"})]
        [TestCase("-0.04", new String[] {"-0.04"})]
        [TestCase("-0.05", new String[] {"-0.05"})]
        [TestCase("-0.06", new String[] {"-0.06"})]
        [TestCase("-0.07", new String[] {"-0.07"})]
        [TestCase("-0.08", new String[] {"-0.08"})]
        [TestCase("-0.09", new String[] {"-0.09"})]
        [TestCase("0.%", new String[] {"0.%"})]
        [TestCase("1.%", new String[] {"1.%"})]
        [TestCase("2.%", new String[] {"2.%"})]
        [TestCase("3.%", new String[] {"3.%"})]
        [TestCase("4.%", new String[] {"4.%"})]
        [TestCase("5.%", new String[] {"5.%"})]
        [TestCase("6.%", new String[] {"6.%"})]
        [TestCase("7.%", new String[] {"7.%"})]
        [TestCase("8.%", new String[] {"8.%"})]
        [TestCase("9.%", new String[] {"9.%"})]
        [TestCase(".0%", new String[] {".0%"})]
        [TestCase(".1%", new String[] {".1%"})]
        [TestCase(".2%", new String[] {".2%"})]
        [TestCase(".3%", new String[] {".3%"})]
        [TestCase(".4%", new String[] {".4%"})]
        [TestCase(".5%", new String[] {".5%"})]
        [TestCase(".6%", new String[] {".6%"})]
        [TestCase(".7%", new String[] {".7%"})]
        [TestCase(".8%", new String[] {".8%"})]
        [TestCase(".9%", new String[] {".9%"})]
        [TestCase("-0.%", new String[] {"-0.%"})]
        [TestCase("-1.%", new String[] {"-1.%"})]
        [TestCase("-2.%", new String[] {"-2.%"})]
        [TestCase("-3.%", new String[] {"-3.%"})]
        [TestCase("-4.%", new String[] {"-4.%"})]
        [TestCase("-5.%", new String[] {"-5.%"})]
        [TestCase("-6.%", new String[] {"-6.%"})]
        [TestCase("-7.%", new String[] {"-7.%"})]
        [TestCase("-8.%", new String[] {"-8.%"})]
        [TestCase("-9.%", new String[] {"-9.%"})]
        [TestCase("-.0%", new String[] {"-.0%"})]
        [TestCase("-.1%", new String[] {"-.1%"})]
        [TestCase("-.2%", new String[] {"-.2%"})]
        [TestCase("-.3%", new String[] {"-.3%"})]
        [TestCase("-.4%", new String[] {"-.4%"})]
        [TestCase("-.5%", new String[] {"-.5%"})]
        [TestCase("-.6%", new String[] {"-.6%"})]
        [TestCase("-.7%", new String[] {"-.7%"})]
        [TestCase("-.8%", new String[] {"-.8%"})]
        [TestCase("-.9%", new String[] {"-.9%"})]
        [TestCase("--.2", new String[] {"-", "-.2"})]
        [TestCase("--.2-", new String[] {"-", "-.2", "-"})]
        [TestCase("--.2-%", new String[] {"-", "-.2", "-", "%"})]
        [TestCase("(", new String[] {"("})]
        [TestCase("((", new String[] {"(", "("})]
        [TestCase("(((", new String[] {"(", "(", "("})]
        [TestCase(")", new String[] {")"})]
        [TestCase("))", new String[] {")", ")"})]
        [TestCase(")))", new String[] {")", ")", ")"})]
        [TestCase(")(", new String[] {")", "*", "("})]
        [TestCase(")()", new String[] {")", "*", "(", ")"})]
        [TestCase("()(", new String[] {"(", ")", "*", "("})]
        [TestCase(")()(", new String[] {")", "*", "(", ")", "*", "("})]
        [TestCase("())(", new String[] {"(", ")", ")", "*", "("})]
        [TestCase(")(()", new String[] {")", "*", "(", "(", ")"})]
        [TestCase(")())", new String[] {")", "*", "(", ")", ")"})]
        [TestCase("-", new String[] {"-"})]
        [TestCase("+", new String[] {"+"})]
        [TestCase("/", new String[] {"/"})]
        [TestCase("%", new String[] {"%"})]
        [TestCase("mod", new String[] {"mod"})]
        [TestCase("modulo", new String[] {"modulo"})]
        [TestCase("*", new String[] {"*"})]
        [TestCase("**", new String[] {"**"})]
        [TestCase("pow", new String[] {"pow"})]
        [TestCase("2pow", new String[] {"2", "pow"})]
        [TestCase("pow2", new String[] {"pow", "2"})]
        [TestCase("power", new String[] {"power"})]
        [TestCase("2power", new String[] {"2", "power"})]
        [TestCase("power2", new String[] {"power", "2"})]
        [TestCase("powerpow", new String[] {"power", "pow"})]
        [TestCase("powpower", new String[] {"pow", "power"})]
        [TestCase("^", new String[] {"^"})]
        [TestCase("2^", new String[] {"2", "^"})]
        [TestCase("^2", new String[] {"^", "2"})]
        [TestCase("2(", new String[] {"2", "*", "("})]
        [TestCase(")2", new String[] {")", "*", "2"})]
        [TestCase("2)(", new String[] {"2", ")", "*", "("})]
        [TestCase(")(2", new String[] {")", "*", "(", "2"})]
        [TestCase("2)(2", new String[] {"2", ")", "*", "(", "2"})]
        [TestCase("--0", new String[] {"-", "-0"})]
        [TestCase("--0%", new String[] {"-", "-0%"})]
        [TestCase("--0.", new String[] {"-", "-0."})]
        [TestCase("--.0", new String[] {"-", "-.0"})]
        [TestCase("--0.%", new String[] {"-", "-0.%"})]
        [TestCase("--.0%", new String[] {"-", "-.0%"})]
        [TestCase("-2-+", new String[] {"-2", "-", "+"})]
        [TestCase("--2+", new String[] {"-", "-2", "+"})]
        [TestCase("-2", new String[] {"-2"})]
        [TestCase("2+", new String[] {"2", "+"})]
        [TestCase("2.+", new String[] {"2.", "+"})]
        [TestCase("2.-+", new String[] {"2.", "-", "+"})]
        [TestCase("2/", new String[] {"2", "/"})]
        [TestCase("2./", new String[] {"2.", "/"})]
        [TestCase("2mod", new String[] {"2", "mod"})]
        [TestCase("2modulo", new String[] {"2", "modulo"})]
        [TestCase("2.mod", new String[] {"2.", "mod"})]
        [TestCase("2.modulo", new String[] {"2.", "modulo"})]
        [TestCase("2*", new String[] {"2", "*"})]
        [TestCase("2.*", new String[] {"2.", "*"})]
        [TestCase("2.%*", new String[] {"2.%", "*"})]
        [TestCase("2**", new String[] {"2", "**"})]
        [TestCase("2.**", new String[] {"2.", "**"})]
        [TestCase("+2", new String[] {"+", "2"})]
        [TestCase("2-", new String[] {"2", "-"})]
        [TestCase("/2", new String[] {"/", "2"})]
        [TestCase("mod2", new String[] {"mod", "2"})]
        [TestCase("modulo2", new String[] {"modulo", "2"})]
        [TestCase("*2", new String[] {"*", "2"})]
        [TestCase("**2", new String[] {"**", "2"})]
        [TestCase("2**", new String[] {"2", "**"})]
        public void CreateInfixTokens_TokenSequence_ReturnsExpectedTokens(string tokenSeq, string[] expectedTokens)
        => Assert.IsTrue(CalculatorTests.IsEqualValue(Program.CreateInfixTokens(tokenSeq), expectedTokens));

        [TestCase("     2 + 2         ", new String[] {"2", "+", "2"})]
        [TestCase("     2+2", new String[] {"2", "+", "2"})]
        [TestCase("2.-2", new String[] {"2.", "-", "2"})]

        [TestCase("4.(2+2)", new String[] {"4.", "*", "(", "2", "+", "2", ")"})]
        [TestCase(".4(2+2)", new String[] {".4", "*", "(", "2", "+", "2", ")"})]
        [TestCase("(2+2).4", new String[] {"(", "2", "+", "2", ")", "*", ".4"})]
        [TestCase("(2+2)-.4", new String[] {"(", "2", "+", "2", ")", "-", ".4"})]
        [TestCase("(2+2)--.4", new String[] {"(", "2", "+", "2", ")", "-", "-.4"})]

        [TestCase("1+6       ", new String[] {"1", "+", "6"})]
        [TestCase("4+2", new String[] {"4", "+", "2"})]
        [TestCase("6+4", new String[] {"6", "+", "4" })]
        [TestCase("2+7+8+1+2", new String[] {"2", "+", "7", "+", "8", "+", "1", "+", "2" })]
        [TestCase("3+3+3+3+3+3+3+3+3", new String[] {"3", "+", "3", "+", "3", "+", "3", "+", "3", "+", "3", "+", "3", "+", "3", "+", "3"})]
        [TestCase("3+-3+3+-3+3+-3+3+-3+-3", new String[] {"3", "+", "-3", "+", "3", "+", "-3", "+", "3", "+", "-3", "+", "3", "+", "-3", "+", "-3"})]
        [TestCase("1+-5+2", new String[] {"1", "+", "-5", "+", "2"})]
        [TestCase("-1+-7+-3", new String[] {"-1", "+", "-7", "+", "-3"})]
        [TestCase("5-6", new String[] {"5", "-", "6"})]
        [TestCase("3/1", new String[] {"3", "/", "1"})]
        [TestCase("1/5/7", new String[] {"1", "/", "5", "/", "7"})]
        [TestCase("2/3/2", new String[] {"2", "/", "3", "/", "2"})]
        [TestCase("5*2", new String[] {"5", "*", "2"})]
        [TestCase("2**7", new String[] {"2", "**", "7"})]
        [TestCase("7**3", new String[] {"7", "**", "3"})]
        [TestCase("-5**8", new String[] {"-5", "**", "8"})]
        [TestCase("2*2*2", new String[] {"2", "*", "2", "*", "2"})]
        [TestCase("2**2**2", new String[] {"2", "**", "2", "**", "2"})]
        [TestCase("-2**2**-2", new String[] {"-2", "**", "2", "**", "-2"})]
        [TestCase("2^7", new String[] {"2", "^", "7"})]
        [TestCase("3^4^6", new String[] {"3", "^", "4", "^", "6"})]
        [TestCase("1%4", new String[] {"1", "%", "4"})]
        [TestCase("4mod4", new String[] {"4", "mod", "4"})]
        [TestCase("4modulo4", new String[] {"4", "modulo", "4"})]
        [TestCase("(7+2)*1", new String[] {"(", "7", "+", "2", ")", "*", "1"})]

        [TestCase("(    7  +  2)  * 1", new String[] {"(", "7", "+", "2", ")", "*", "1"})]
        [TestCase("(    7  + 3   2)  * 1", new String[] {"(", "7", "+", "32", ")", "*", "1"})]
        [TestCase("(  1  7  +        2)  * 1", new String[] {"(", "17", "+", "2", ")", "*", "1"})]
        [TestCase("8-2+(3*4)/2^2", new String[] {"8", "-", "2", "+", "(", "3", "*", "4", ")", "/", "2", "^", "2"})]
        [TestCase("-2.%%-.2%2", new String[] {"-2.%", "%", "-.2", "%", "2"})]
        [TestCase("3%.2%", new String[] {"3", "%", ".2%"})]
        [TestCase("-4.%-2.", new String[] {"-4.", "%", "-2."})]
        [TestCase("5.+-2%^8", new String[] {"5.", "+", "-2%", "^", "8"})]

        [TestCase("6/2(1+2)", new String[] {"6", "/", "2", "*", "(", "1", "+", "2", ")"})]
        [TestCase("8/2(2+2)", new String[] {"8", "/", "2", "*", "(", "2", "+", "2", ")"})]
        [TestCase("6+9+4^2", new String[] {"6", "+", "9", "+", "4", "^", "2"})]
        [TestCase("5*(6^2-2)", new String[] {"5", "*", "(", "6", "^", "2", "-", "2", ")"})]
        [TestCase("4*8^2+11", new String[] {"4", "*", "8", "^", "2", "+", "11"})]
        [TestCase("46+(8*4)/2", new String[] {"46", "+", "(", "8", "*", "4", ")", "/", "2"})]
        [TestCase("6+9+(4*2+4^2)", new String[] {"6", "+", "9", "+", "(", "4", "*", "2", "+", "4", "^", "2", ")"})]
        [TestCase("7^2*(25+10/5)-13", new String[] {"7", "^", "2", "*", "(", "25", "+", "10", "/", "5", ")", "-", "13"})]
        [TestCase("10-7*(5+2)+7^2", new String[] {"10", "-", "7", "*", "(", "5", "+", "2", ")", "+", "7", "^", "2"})]
        [TestCase("5-3*(2^3-5+7*(-3))", new String[] {"5", "-", "3", "*", "(", "2", "^", "3", "-", "5", "+", "7", "*", "(", "-3", ")", ")"})]
        [TestCase("2*(1+(4*(2+1)+3))", new String[] {"2", "*", "(", "1", "+", "(", "4", "*", "(", "2", "+", "1", ")", "+", "3", ")", ")"})]
        [TestCase("(3*5^2/15)-(5-2^2)", new String[] {"(", "3", "*", "5", "^", "2", "/", "15", ")", "-", "(", "5", "-", "2", "^", "2", ")"})]
        [TestCase("((3+2)^2+3)-3+3^2", new String[] {"(", "(", "3", "+", "2", ")", "^", "2", "+", "3", ")", "-", "3", "+", "3", "^", "2"})]
        [TestCase("(18/3)^2+((13+7)*5^2)", new String[] {"(", "18", "/", "3", ")", "^", "2", "+", "(", "(", "13", "+", "7", ")", "*", "5", "^", "2", ")"})]
        [TestCase("(18/3)**2+((13+7)*5**2)", new String[] {"(", "18", "/", "3", ")", "**", "2", "+", "(", "(", "13", "+", "7", ")", "*", "5", "**", "2", ")"})]
        [TestCase("78+(30-0.5(28+8))/6", new String[] {"78", "+", "(", "30", "-", "0.5", "*", "(", "28", "+", "8", ")", ")", "/", "6"})]
        [TestCase("(5.9-5.3)*7.2+1.4^2", new String[] {"(", "5.9", "-", "5.3", ")", "*", "7.2", "+", "1.4", "^", "2"})]
        [TestCase("(2.1^2+5.2-7.2)*7.1", new String[] {"(", "2.1", "^", "2", "+", "5.2", "-", "7.2", ")", "*", "7.1" })]
        [TestCase("145+(((123-456)789)+147-753)", new String[] {"145", "+", "(", "(", "(", "123", "-", "456", ")", "*", "789", ")", "+", "147", "-", "753", ")"})]
        [TestCase("-145+(((123-456)789)+147-753)", new String[] {"-145", "+", "(", "(", "(", "123", "-", "456", ")", "*", "789", ")", "+", "147", "-", "753", ")"})]
        [TestCase("145+(((123-456)*789)+147-753)", new String[] {"145", "+", "(", "(", "(", "123", "-", "456", ")", "*", "789", ")", "+", "147", "-", "753", ")"})]
        [TestCase("-145+(((123-456)*789)+147-753)", new String[] {"-145", "+", "(", "(", "(", "123", "-", "456", ")", "*", "789", ")", "+", "147", "-", "753", ")"})]
        [TestCase("4+5+(1*2+7power2)", new String[] {"4", "+", "5", "+", "(", "1", "*", "2", "+", "7", "power", "2", ")"})]
        [TestCase("3pow5*(245+10/5)-13", new String[] {"3", "pow", "5", "*", "(", "245", "+", "10", "/","5", ")", "-", "13"})]
        [TestCase("10-756*(5+2)+745**2", new String[] {"10", "-", "756", "*", "(", "5", "+", "2", ")", "+", "745", "**", "2"})]
        public void CreateInfixTokens_ValidExpressions_ReturnsExpectedTokens(string validExp, string[] expectedTokens)
        => Assert.IsTrue(CalculatorTests.IsEqualValue(Program.CreateInfixTokens(validExp), expectedTokens));

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("    ")]
        [TestCase("a")]
        [TestCase("e")]
        [TestCase("i")]
        [TestCase("o")]
        [TestCase("u")]
        [TestCase("@")]
        [TestCase("~")]
        [TestCase("#")]
        [TestCase("fish")]
        [TestCase("dog")]
        [TestCase("sakasksa")]
        [TestCase("oisakosakoplaskosa")]
        [TestCase("Serio is best programmer in the world")]
        [TestCase("hello world")]
        [TestCase("=")]
        [TestCase(".")]
        [TestCase("\\")]
        [TestCase("!")]
        [TestCase("?")]
        [TestCase("?!?!?!")]
        [TestCase("!£&_")]
        [TestCase("I")]
        [TestCase("IV")]
        [TestCase("X")]
        [TestCase("2!+2")]
        [TestCase(".2+!2")]
        [TestCase("2+2!+2")]
        [TestCase("2+2+ssd2")]
        [TestCase("2z--2")]
        [TestCase("-2-?2")]
        [TestCase("2*\\*2")]
        [TestCase("2___2")]
        [TestCase("2\\2")]
        [TestCase("2+2apples")]
        [TestCase("2*2*&*2")]
        [TestCase("..2")]
        [TestCase(".-.2")]
        [TestCase("4.7.")]
        [TestCase(".4.7")]
        [TestCase("8-2+(3..4)/2^2")]
        [TestCase("-.2.2")]
        [TestCase("-2..2")]
        public void CreateInfixTokens_InvalidCharacters_ReturnsEmpty(string invalidCharSeq)
        => Assert.AreEqual(Program.CreateInfixTokens(invalidCharSeq).Count, 0);
        
        [Test]
        public void CreateInfixTokens_NullExpression_ReturnsEmpty()
        => Assert.AreEqual(Program.CreateInfixTokens(null).Count, 0);
    }
}
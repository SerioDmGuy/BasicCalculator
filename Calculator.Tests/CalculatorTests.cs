using System;
using System.Text.RegularExpressions;

namespace Calculator.Tests
{
    public class CalculatorTests
    {
        public static bool IsEqualValue(MatchCollection collection, string[] strArray)
        {
            if (collection.Count != strArray.Length)
                return false;

            int index = 0;
            foreach (Match match in collection)
                if (match.Value == strArray[index]) index++;
                else return false;
            return true;
        }
    }
}
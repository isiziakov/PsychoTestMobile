using PsychoTestAndroid.ResultsCalculator.Model;
using System.Collections.Generic;

namespace PsychoTestAndroid.ResultsCalculator
{
    public static class ParseFormula
    {
        public static string Parse(string formula, List<Scale> results)
        {
            int firstIndex;
            do
            {
                firstIndex = formula.IndexOf("GRADNUM");
                if (firstIndex != -1)
                {
                    string idTestScale = "";
                    int lastIndex;
                    for (lastIndex = firstIndex + 11; lastIndex < formula.Length; lastIndex++)
                        if (formula[lastIndex] != ')')
                            idTestScale += formula[lastIndex];
                        else break;

                    int value = 0;
                    foreach (var r in results)
                        if (r.IdTestScale == idTestScale)
                        {
                            value = (int)r.GradationNumber;
                            break;
                        }
                    formula = formula.Remove(firstIndex, lastIndex - firstIndex + 1);
                    formula = formula.Insert(firstIndex, value.ToString());
                }
            } while (firstIndex != -1);

            do
            {
                firstIndex = formula.IndexOf("Scale");
                if (firstIndex != -1)
                {
                    string idTestScale = "";
                    int lastIndex;
                    for (lastIndex = firstIndex + 9; lastIndex < formula.Length; lastIndex++)
                        if (formula[lastIndex] != ')')
                            idTestScale += formula[lastIndex];
                        else break;

                    int value = 0;
                    foreach (var r in results)
                        if (r.IdTestScale == idTestScale)
                        {
                            value = (int)r.Scores;
                            break;
                        }
                    formula = formula.Remove(firstIndex, lastIndex - firstIndex + 1);
                    formula = formula.Insert(firstIndex, value.ToString());
                }
            } while (firstIndex != -1);

            do
            {
                firstIndex = formula.IndexOf("STSUMM");
                if (firstIndex != -1)
                {
                    string idTestScale = "";
                    int lastIndex;
                    for (lastIndex = firstIndex + 10; lastIndex < formula.Length; lastIndex++)
                        if (formula[lastIndex] != ')')
                            idTestScale += formula[lastIndex];
                        else break;

                    int value = 0;
                    foreach (var r in results)
                        if (r.IdTestScale == idTestScale)
                        {
                            value = (int)r.Scores;
                            break;
                        }
                    formula = formula.Remove(firstIndex, lastIndex - firstIndex + 1);
                    formula = formula.Insert(firstIndex, value.ToString());
                }
            } while (firstIndex != -1);

            do
            {
                firstIndex = formula.IndexOf("abs");
                if (firstIndex != -1)
                {
                    int lastIndex;
                    string shortFormula = "";
                    for (lastIndex = firstIndex + 4; lastIndex < formula.Length; lastIndex++)
                        if (formula[lastIndex] != ')')
                            shortFormula += formula[lastIndex];
                        else break;
                    double value = Calculator.Calculate(shortFormula);
                    if (value < 0)
                        value *= -1;

                    formula = formula.Remove(firstIndex, lastIndex - firstIndex + 1);
                    formula = formula.Insert(firstIndex, value.ToString());
                }
            } while (firstIndex != -1);

            do
            {
                firstIndex = formula.IndexOf("==");
                if (firstIndex != -1)
                {
                    int leftPartStart, rightPartFinish;
                    string leftPart = "", rightPart = "";
                    for (leftPartStart = firstIndex - 1; leftPartStart > -1; leftPartStart--)
                        if (formula[leftPartStart] != '(')
                            leftPart += formula[leftPartStart];
                        else break;
                    for (rightPartFinish = firstIndex + 2; rightPartFinish < formula.Length; rightPartFinish++)
                        if (formula[rightPartFinish] != ')')
                            rightPart += formula[rightPartFinish];
                        else break;

                    int value = 0;
                    if (leftPart == rightPart)
                        value = 1;

                    formula = formula.Remove(leftPartStart, rightPartFinish - leftPartStart + 1);
                    formula = formula.Insert(leftPartStart, value.ToString());
                }
            } while (firstIndex != -1);

            do
            {
                firstIndex = formula.IndexOf("\r");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 1);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("\n");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 1);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("\t");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 1);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("out");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 7);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("if ");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 3);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("else");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 4);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("{");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 1);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("}");
                if (firstIndex != -1)
                    formula = formula.Remove(firstIndex, 1);
            } while (firstIndex != -1);
            do
            {
                firstIndex = formula.IndexOf("||");
                if (firstIndex != -1)
                {
                    int r = 0;
                    if (formula[firstIndex - 1] == '1' || formula[firstIndex + 2] == '1')
                        r = 1;
                    formula = formula.Remove(firstIndex - 1, 4);
                    formula = formula.Insert(firstIndex - 1, r.ToString());
                }
            } while (firstIndex != -1);


            return formula;
        }
    }
}
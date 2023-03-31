using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.ResultsCalculator
{
    public static class Calculator
    {
        private const string numberChars = "01234567890.";
        private const string operatorChars = "^*/+-";

        public static double Calculate(string expression)
        {
            return EvaluateParenthesis(expression);
        }

        public static double EvaluateParenthesis(string expression) //
        {
            string planarExpression = expression;
            while (planarExpression.IndexOf('(') > -1)
            {
                int clauseStart = planarExpression.IndexOf('(') + 1;
                int clauseEnd = IndexOfRightParenthesis(planarExpression, clauseStart);
                string clause = planarExpression.Substring(clauseStart, clauseEnd - clauseStart);
                planarExpression = planarExpression.Replace("(" + clause + ")", EvaluateParenthesis(clause).ToString());
            }
            return Evaluate(planarExpression);
        }

        public static int IndexOfRightParenthesis(string expression, int start) //
        {
            int c = 1;
            for (int i = start; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '(': c++; break;
                    case ')': c--; break;
                }
                if (c == 0) return i;
            }
            return -1;
        }

        public static double Evaluate(string expression) //
        {
            string normalExpression = expression.Replace(" ", "").Replace(",", ".");
            List<char> operators = normalExpression.Split(numberChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToList();
            List<double> numbers = normalExpression.Split(operatorChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => double.Parse(x, System.Globalization.CultureInfo.InvariantCulture)).ToList();

            foreach (char o in operatorChars)
            {
                for (int i = 0; i < operators.Count; i++)
                {
                    if (operators[i] == o)
                    {
                        double result = Calc(numbers[i], numbers[i + 1], o);
                        numbers[i] = result;
                        numbers.RemoveAt(i + 1);
                        operators.RemoveAt(i);
                        i--;
                    }
                }
            }
            return numbers[0];
        }

        public static double Calc(double left, double right, char oper) //
        {
            switch (oper)
            {
                case '+': return left + right;
                case '-': return left - right;
                case '*': return left * right;
                case '/': return left / right;
                case '^': return Math.Pow(left, right);
                default: return 0;
            }
        }
    }
}
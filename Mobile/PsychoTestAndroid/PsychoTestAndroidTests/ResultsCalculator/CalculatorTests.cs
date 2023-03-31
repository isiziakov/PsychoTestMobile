using Microsoft.VisualStudio.TestTools.UnitTesting;
using PsychoTestAndroid.ResultsCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTestAndroid.ResultsCalculator.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestMethod()]
        public void CalcTest1()
        {
            double left = 2.0;
            double right = 3.0;
            char oper = ' ';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(0, res);
        }

        [TestMethod()]
        public void CalcTest2()
        {
            double left = 2.0;
            double right = 3.0;
            char oper = '+';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(5.0, res);
        }

        [TestMethod()]
        public void CalcTest3()
        {
            double left = 2.0;
            double right = 3.0;
            char oper = '-';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(-1.0, res);
        }

        [TestMethod()]
        public void CalcTest4()
        {
            double left = 2.0;
            double right = 3.0;
            char oper = '*';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(6.0, res);
        }

        [TestMethod()]
        public void CalcTest5()
        {
            double left = 3.0;
            double right = 2.0;
            char oper = '/';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(1.5, res);
        }

        [TestMethod()]
        public void CalcTest6()
        {
            double left = 2.0;
            double right = 3.0;
            char oper = '^';

            var res = Calculator.Calc(left, right, oper);

            Assert.AreEqual(8.0, res);
        }

        [TestMethod()]
        public void IndexOfRightParenthesisTest1()
        {
            string expression = "1";
            int start = 0;

            var res = Calculator.IndexOfRightParenthesis(expression, start);

            Assert.AreEqual(-1, res);
        }

        [TestMethod()]
        public void IndexOfRightParenthesisTest2()
        {
            string expression = "((1+2)*2+3)";
            int start = 1;

            var res = Calculator.IndexOfRightParenthesis(expression, start);

            Assert.AreEqual(10, res);
        }

        [TestMethod()]
        public void IndexOfRightParenthesisTest3()
        {
            string expression = "((1+2)*2+3)";
            int start = 2;

            var res = Calculator.IndexOfRightParenthesis(expression, start);

            Assert.AreEqual(5, res);
        }

        [TestMethod()]
        public void EvaluateParenthesisTest1()
        {
            string expression = "((1+2)*2+3)";

            var res = Calculator.EvaluateParenthesis(expression);

            Assert.AreEqual(1, res);
        }
    }
}
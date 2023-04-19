using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator;
using PsychoTestAndroid.ResultsCalculator.Calculators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTestAndroidTests.ResultsCalculator
{
    [TestClass()]
    public class IntegrationTests
    {
        [TestMethod()] 
        public void CalculationByFormulasTest()
        {
            CalcInfo test = new CalcInfo();
            test.Groups = JObject.Parse(File.ReadAllText("test.txt"));
            CalculatorLusher calc = new CalculatorLusher();
            var mock = new Mock<Calculator.ICalc>();
            var input1 = "1 + 2 + 3 + 4 + 5 + 6 + 7 + 8";
            var input2 = "(18 - 0 - 0) / (18 - 0 - 0)";
            mock.Setup(m => m.CalculateTest(input1)).Returns(36.0);
            mock.Setup(m => m.CalculateTest(input2)).Returns(1.0);
            mock.Setup(m => m.CalculateTest(It.Is<string>(u => u != input1 && u != input2))).Returns(0.0);
            Calculator.CalcTest = mock.Object;

            calc.CalculationByFormulas(test);
            var res = JsonConvert.SerializeObject(calc.patientResult).ToString();

            var currect = "{\"scales\":[{\"idTestScale\":\"PM1\",\"idNormScale\":null,\"name\":\"Среднее место (1 - темно-синий) \",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM2\",\"idNormScale\":null,\"name\":\"Среднее место (2 - сине-зеленый)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM3\",\"idNormScale\":null,\"name\":\"Среднее место (3 - оранжево-красный)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM4\",\"idNormScale\":null,\"name\":\"Среднее место (4 – желтый)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM5\",\"idNormScale\":null,\"name\":\"Среднее место (5 – фиолетовый)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM6\",\"idNormScale\":null,\"name\":\"Среднее место (6 – коричневый)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM7\",\"idNormScale\":null,\"name\":\"Среднее место (7 – черный)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PM0\",\"idNormScale\":null,\"name\":\"Среднее место (0 – серый)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"CO\",\"idNormScale\":\"010000000000_24\",\"name\":\"Cуммарное отклонение от аутогенной нормы (СО)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"VK\",\"idNormScale\":\"010000000000_25\",\"name\":\"Вегетативный коэффициент (ВК)\",\"scores\":1.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"G\",\"idNormScale\":\"010000000000_26\",\"name\":\"Гетерономность-автономность\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"K\",\"idNormScale\":\"010000000000_27\",\"name\":\"Концентричность-эксцентричность\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"VB\",\"idNormScale\":\"010000000000_28\",\"name\":\"Вегетативный баланс\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"LB\",\"idNormScale\":\"010000000000_29\",\"name\":\"Личностный баланс\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"PR\",\"idNormScale\":\"010000000000_30\",\"name\":\"Показатель работоспособности\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"y1\",\"idNormScale\":null,\"name\":\"y1-стрессовый цвет (синий, зеленый, красный, желтый) на 6-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"y2\",\"idNormScale\":null,\"name\":\"y2-стрессовый цвет (синий, зеленый, красный, желтый) на 7-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"y3\",\"idNormScale\":null,\"name\":\"y3-стрессовый цвет (синий, зеленый, красный, желтый) на 8-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"x1\",\"idNormScale\":null,\"name\":\"x1-стрессовый цвет (коричневый, серый, черный) на 1-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"x2\",\"idNormScale\":null,\"name\":\"x2-стрессовый цвет (коричневый, серый, черный) на 2-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"x3\",\"idNormScale\":null,\"name\":\"x3-стрессовый цвет (коричневый, серый, черный) на 3-м месте в любом из выборов\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"s1\",\"idNormScale\":null,\"name\":\"Показатель стресса (C1)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"s2\",\"idNormScale\":null,\"name\":\"Показатель стресса (C2)\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null},{\"idTestScale\":\"s\",\"idNormScale\":\"010000000000_39\",\"name\":\"Показатель стресса\",\"scores\":0.0,\"gradationNumber\":null,\"interpretation\":null}]}";
            Assert.AreEqual(currect, res);
        }
    }
}

using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychoTestAndroid.ResultsCalculator.Calculators
{
    public class CalculatorLusher
    {
        public PatientsResult PatientResult = new PatientsResult();

        public CalculatorLusher()
        {

        }
        public CalculatorLusher(CalcInfo test, TestResult result, Norm norm)
        {
            string[] testResult = result.Answers[0].Answer.Split(' ').Concat(result.Answers[1].Answer.Split(' ')).ToArray();

            // Заполнение первых 16 шкал — порядок цветов.
            AddOrderColor(test, testResult);

            // Расчет по формулам.
            CalculationByFormulas(test);

            // Интерпритация.
            RangeInterpretation(norm, PatientResult.Scales);

            // Удаляем вспомогательные шкалы из окончательного результата.
            RemoveAuxiliaryScales(PatientResult.Scales);
        }

        // Первые 16 шкал — порядок цветов.
        private void AddOrderColor(CalcInfo test, string[] testResult)
        {
            int start = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int i = start; i < start + 8; i++)
                {
                    var scale = new Scale();

                    if (test.Groups["item"][i]["NormID"] != null)
                        scale.IdNormScale = test.Groups["item"][i]["NormID"].ToString();
                    scale.IdTestScale = test.Groups["item"][i]["ID"].ToString();
                    scale.Name = test.Groups["item"][i]["Name"]["#text"].ToString();
                    for (int j = start; j < start + 8; j++)
                    {
                        if (testResult[j] == i.ToString() && i < 8)
                        {
                            scale.Scores = j + 1;
                            break;
                        }
                        if (testResult[j] == (i - 8).ToString() && i >= 8)
                        {
                            scale.Scores = j - 7;
                            break;
                        }
                    }
                    PatientResult.Scales.Add(scale);
                }
                start += 8;
            }
        }

        public void CalculationByFormulas(CalcInfo test)
        {
            for (int i = 16; i < 40; i++)
            {
                var patientScale = new Scale();
                var testScale = test.Groups["item"][i];

                if (testScale["NormID"] != null)
                    patientScale.IdNormScale = testScale["NormID"].ToString();
                patientScale.IdTestScale = testScale["ID"].ToString();
                patientScale.Name = testScale["Name"]["#text"].ToString();

                string formula = testScale["Formula"].ToString();

                // Парсим формулу.
                formula = ParseFormula.Parse(formula, PatientResult.Scales);

                // Расчет баллов по формуле.
                patientScale.Scores = Calculator.Calculate(formula);

                PatientResult.Scales.Add(patientScale);
            }
        }

        // Автоматическая интерпретация результатов.
        private void RangeInterpretation(Norm norm, List<Scale> results)
        {
            // Шкалы из норм.
            JArray normScales = new JArray();
            if (norm.Data["main"]["groups"]["item"]["quantities"]["item"] is JArray)
                foreach (var scale in norm.Data["main"]["groups"]["item"]["quantities"]["item"])
                    normScales.Add(scale);
            else
                normScales.Add(norm.Data["main"]["groups"]["item"]["quantities"]["item"]);


            // Для каждой вычисленной шкалы.
            foreach (var result in results)
            {
                // Если градация еще не определена.
                if (result.GradationNumber == null)
                    foreach (var normScale in normScales)
                    {
                        // Находим соответствующую шкалу из норм. 
                        if (result.Scores != null && result.IdNormScale == normScale["id"].ToString())
                        {
                            // Выбираем все градации шкалы.
                            JArray grads = new JArray();
                            if (normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"] is JArray)
                                foreach (var grad in normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"])
                                    grads.Add(grad);
                            else
                                grads.Add(normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"]);



                            // Для каждой градации.
                            foreach (var bsonGrad in grads)
                            {
                                var grad = JObject.Parse(bsonGrad.ToString());

                                // Если обе границы inf.
                                if (grad["lowerformula"]["ftext"].ToString() == "-inf" && grad["upperformula"]["ftext"].ToString() == "+inf")
                                {
                                    if (grad["comment"]["#text"] != null)
                                        result.Interpretation = grad["comment"]["#text"].ToString();
                                    result.GradationNumber = int.Parse(grad["number"].ToString());
                                }
                                else
                                // Если слева -inf, справа число.
                                if (grad["lowerformula"]["ftext"].ToString() == "-inf" && grad["upperformula"]["ftext"].ToString() != "+inf")
                                {
                                    if (result.Scores <= double.Parse(grad["upperformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.Interpretation = grad["comment"]["#text"].ToString();
                                        result.GradationNumber = Int32.Parse(grad["number"].ToString());
                                    }
                                }
                                else
                                // Если справа +inf, слева число.
                                if (grad["lowerformula"]["ftext"].ToString() != "-inf" && grad["upperformula"]["ftext"].ToString() == "+inf")
                                {
                                    if (result.Scores > double.Parse(grad["lowerformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.Interpretation = grad["comment"]["#text"].ToString();
                                        result.GradationNumber = Int32.Parse(grad["number"].ToString());
                                    }
                                }
                                else
                                // С обеих сторон числа.
                                if (grad["lowerformula"]["ftext"].ToString() != "-inf" && grad["upperformula"]["ftext"].ToString() != "+inf")
                                {
                                    if (result.Scores > double.Parse(grad["lowerformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture) && result.Scores <= (int)double.Parse(grad["upperformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.Interpretation = grad["comment"]["#text"].ToString();
                                        result.GradationNumber = Int32.Parse(grad["number"].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    }
            }
        }

        private void RemoveAuxiliaryScales(List<Scale> scales)
        {
            // Удаляем первые 16 значений — место цветов в двух выборках.
            // Вместо них оставим средние значения мест (среднее по ряду выборов место каждого цвета).
            scales.RemoveRange(0, 16);

            // Для шкал y1,y2,y3,x1,x2,x3 стрессовых цветов сохраняем только вариант, неравный 0 в соответствии с местом.
            for (int i = 15; i < 21; i++)
                if (scales[i].Scores == 0)
                    scales[i].Scores = null;

            // Убираем 2 промежуточных значения для рассчета показателя стресса.
            scales.RemoveRange(scales.Count() - 3, 2);
        }
    }

}
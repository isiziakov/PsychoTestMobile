using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.ResultsCalculator.Calculators
{
    public class CalculatorLusher
    {
        public PatientsResult patientResult = new PatientsResult();

        public CalculatorLusher(CalcInfo test, TestResult result, BsonDocument norm)
        {
            string[] testResult = result.Answers[0].Answer.Split(' ').Concat(result.Answers[1].Answer.Split(' ')).ToArray();

            //заполнение первых 16 шкал — порядок цветов
            AddOrderColor(test, testResult);

            //расчет по формулам
            CalculationByFormulas(test);

            //интерпритация
            RangeInterpretation(norm, patientResult.scales);

            //Удаляем вспомогательные шкалы из окончательного результата 
            RemoveAuxiliaryScales(patientResult.scales);
        }

        //первые 16 шкал — порядок цветов
        private void AddOrderColor(CalcInfo test, string[] testResult)
        {
            int start = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int i = start; i < start + 8; i++)
                {
                    var scale = new Scale();

                    if (test.Groups["item"][i]["NormID"] != null)
                        scale.idNormScale = test.Groups["item"][i]["NormID"].ToString();
                    scale.idTestScale = test.Groups["item"][i]["ID"].ToString();
                    scale.name = test.Groups["item"][i]["Name"]["#text"].ToString();
                    for (int j = start; j < start + 8; j++)
                    {
                        if (testResult[j] == i.ToString() && i < 8)
                        {
                            scale.scores = j + 1;
                            break;
                        }
                        if (testResult[j] == (i - 8).ToString() && i >= 8)
                        {
                            scale.scores = j - 7;
                            break;
                        }
                    }
                    patientResult.scales.Add(scale);
                }
                start += 8;
            }
        }

        private void CalculationByFormulas(CalcInfo test)
        {
            for (int i = 16; i < 40; i++)
            {
                var patientScale = new Scale();
                var testScale = test.Groups["item"][i];

                if (testScale["NormID"] != null)
                    patientScale.idNormScale = testScale["NormID"].ToString();
                patientScale.idTestScale = testScale["ID"].ToString();
                patientScale.name = testScale["Name"]["#text"].ToString();

                string formula = testScale["Formula"].ToString();

                //парсим формулу
                formula = ParseFormula.Parse(formula, patientResult.scales);

                //расчет баллов по формуле
                patientScale.scores = Calculator.Calculate(formula);

                patientResult.scales.Add(patientScale);
            }
        }

        //автоматическая интерпретация результатов
        private void RangeInterpretation(BsonDocument norm, List<Scale> results)
        {
            //шкалы из норм
            BsonArray normScales = new BsonArray();
            if (norm["main"]["groups"]["item"]["quantities"]["item"] is BsonArray)
                foreach (var scale in norm["main"]["groups"]["item"]["quantities"]["item"].AsBsonArray)
                    normScales.Add(scale);
            else
                normScales.Add(norm["main"]["groups"]["item"]["quantities"]["item"]);


            //Для каждой вычисленной шкалы
            foreach (var result in results)
            {
                //если градация еще не определена
                if (result.gradationNumber == null)
                    foreach (var normScale in normScales)
                    {
                        //находим соответствующую шкалу из норм 
                        if (result.scores != null && result.idNormScale == normScale["id"].AsString)
                        {
                            //выбираем все градации шкалы
                            BsonArray grads = new BsonArray();
                            if (normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"] is BsonArray)
                                foreach (var grad in normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"].AsBsonArray)
                                    grads.Add(grad);
                            else
                                grads.Add(normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"]);



                            //Для каждой градации
                            foreach (var bsonGrad in grads)
                            {
                                var grad = JObject.Parse(JsonConvert.SerializeObject(BsonTypeMapper.MapToDotNetValue(bsonGrad)));

                                //если обе границы inf
                                if (grad["lowerformula"]["ftext"].ToString() == "-inf" && grad["upperformula"]["ftext"].ToString() == "+inf")
                                {
                                    if (grad["comment"]["#text"] != null)
                                        result.interpretation = grad["comment"]["#text"].ToString();
                                    result.gradationNumber = Int32.Parse(grad["number"].ToString());
                                }
                                else
                                //если слева -inf, справа число
                                if (grad["lowerformula"]["ftext"].ToString() == "-inf" && grad["upperformula"]["ftext"].ToString() != "+inf")
                                {
                                    if (result.scores <= double.Parse(grad["upperformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.interpretation = grad["comment"]["#text"].ToString();
                                        result.gradationNumber = Int32.Parse(grad["number"].ToString());
                                    }
                                }
                                else
                                //если справа +inf, слева число
                                if (grad["lowerformula"]["ftext"].ToString() != "-inf" && grad["upperformula"]["ftext"].ToString() == "+inf")
                                {
                                    if (result.scores > double.Parse(grad["lowerformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.interpretation = grad["comment"]["#text"].ToString();
                                        result.gradationNumber = Int32.Parse(grad["number"].ToString());
                                    }
                                }
                                else
                                //c обеих сторон числа
                                if (grad["lowerformula"]["ftext"].ToString() != "-inf" && grad["upperformula"]["ftext"].ToString() != "+inf")
                                {
                                    if (result.scores > double.Parse(grad["lowerformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture) && result.scores <= (int)double.Parse(grad["upperformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
                                    {
                                        if (grad["comment"]["#text"] != null)
                                            result.interpretation = grad["comment"]["#text"].ToString();
                                        result.gradationNumber = Int32.Parse(grad["number"].ToString());
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
            //удаляем первые 16 значений — место цветов в двух выборках.
            //Вместо них оставим средние значения мест (среднее по ряду выборов место каждого цвета)
            scales.RemoveRange(0, 16);

            //для шкал y1,y2,y3,x1,x2,x3 стрессовых цветов сохраняем только вариант, неравный 0 в соответствии с местом
            for (int i = 15; i < 21; i++)
                if (scales[i].scores == 0)
                    scales[i].scores = null;

            //убираем 2 промежуточных значения для рассчета показателя стресса
            scales.RemoveRange(scales.Count() - 3, 2);
        }
    }

}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.ResultsCalculator.Calculators
{
    public class CalculatorBase
    {
        public PatientsResult patientResult = new PatientsResult();

        public CalculatorBase(CalcInfo test, TestResult result, Norm norm)
        {
            //подсчет баллов по шкалам
            Dictionary<string, double> sum = Scorting(test, result, patientResult.scales);

            //добавление шкал пациенту
            AddScales(sum, test);

            //автоматическая интерпретация результатов
            //нахождение диапазонов
            RangeInterpretation(norm, patientResult.scales);
            //рассчет по формулам
            CalculationByFormulas(test, patientResult.scales, norm);
        }


        //подсчет баллов по шкалам
        private Dictionary<string, double> Scorting(CalcInfo test, TestResult result, List<Scale> patientScales)
        {
            //id шкалы - сумма по шкале 
            Dictionary<string, double> scales = new Dictionary<string, double>();
            foreach (var answer in result.Answers)
            {
                var id = int.Parse(answer.Id);
                //Если вопрос с выбором одного из вариантов ответа
                if (Int32.Parse(test.Questions["item"][id]["Question_Choice"].ToString()) == 1)
                {
                    if (answer.Answer != "")
                        if (test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"] != null)
                            if (test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"].ToString() != "")
                            {
                                //id шкалы
                                string scale = "";
                                if (test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"]["item"] is JArray)
                                    foreach (var s in test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"]["item"])
                                    {
                                        scale = s["ScID"].ToString();
                                        if (scales.ContainsKey(scale))
                                            scales[scale] += double.Parse(s["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                        else
                                        {
                                            scales[scale] = double.Parse(s["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                    }
                                else
                                {
                                    scale = test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"]["item"]["ScID"].ToString();
                                    if (scales.ContainsKey(scale))
                                        scales[scale] += double.Parse(test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"]["item"]["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                    else
                                    {
                                        scales[scale] = double.Parse(test.Questions["item"][id]["Answers"]["item"][Int32.Parse(answer.Answer)]["Weights"]["item"]["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                }
                //Если вопрос с вводом своего ответа
                else foreach (var ans in test.Questions["item"][id]["Answers"]["item"])
                    {
                        if (answer.Answer == ans["Name"]["#text"].ToString())
                            if (ans["Weights"] != null)
                            {
                                if (ans["Weights"]["item"] is JArray)
                                {
                                    foreach (var i in ans["Weights"]["item"])
                                        if (scales.ContainsKey(i["ScID"].ToString()))
                                            scales[i["ScID"].ToString()] += double.Parse(i["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                        else
                                        {
                                            scales[i["ScID"].ToString()] = double.Parse(i["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                }
                                else
                                {
                                    if (scales.ContainsKey(ans["Weights"]["item"]["ScID"].ToString()))
                                        scales[ans["Weights"]["item"]["ScID"].ToString()] += double.Parse(ans["Weights"]["item"]["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                    else
                                    {
                                        scales[ans["Weights"]["item"]["ScID"].ToString()] = double.Parse(ans["Weights"]["item"]["Weights"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                    }
            }
            return scales;
        }


        //добавление шкал пациенту
        private void AddScales(Dictionary<string, double> sum, CalcInfo test)
        {
            if (test.Groups["item"] is JArray)
                foreach (var scale in test.Groups["item"])
                    if (sum.ContainsKey(scale["ID"].ToString()))
                        if (scale["NormID"] != null)
                            patientResult.scales.Add(new Scale() { idTestScale = scale["ID"].ToString(), idNormScale = scale["NormID"].ToString(), name = scale["Name"]["#text"].ToString(), scores = sum[scale["ID"].ToString()] });
                        else
                            patientResult.scales.Add(new Scale() { idTestScale = scale["ID"].ToString(), name = scale["Name"]["#text"].ToString(), scores = sum[scale["ID"].ToString()] });
                    else
                        if (scale["NormID"] != null)
                        patientResult.scales.Add(new Scale() { idTestScale = scale["ID"].ToString(), idNormScale = scale["NormID"].ToString(), name = scale["Name"]["#text"].ToString() });
                    else
                        patientResult.scales.Add(new Scale() { idTestScale = scale["ID"].ToString(), name = scale["Name"]["#text"].ToString() });
            else
            if (sum.ContainsKey(test.Groups["item"]["ID"].ToString()))
                if (test.Groups["item"]["NormID"] != null)
                    patientResult.scales.Add(new Scale() { idTestScale = test.Groups["item"]["ID"].ToString(), idNormScale = test.Groups["item"]["NormID"].ToString(), name = test.Groups["item"]["Name"]["#text"].ToString(), scores = sum[test.Groups["item"]["ID"].ToString()] });
                else
                    patientResult.scales.Add(new Scale() { idTestScale = test.Groups["item"]["ID"].ToString(), name = test.Groups["item"]["Name"]["#text"].ToString(), scores = sum[test.Groups["item"]["ID"].ToString()] });
            else
                if (test.Groups["item"]["NormID"] != null)
                patientResult.scales.Add(new Scale() { idTestScale = test.Groups["item"]["ID"].ToString(), idNormScale = test.Groups["item"]["NormID"].ToString(), name = test.Groups["item"]["Name"]["#text"].ToString() });
            else
                patientResult.scales.Add(new Scale() { idTestScale = test.Groups["item"]["ID"].ToString(), name = test.Groups["item"]["Name"]["#text"].ToString() });
        }


        //автоматическая интерпретация результатов
        private void RangeInterpretation(Norm norm, List<Scale> results)
        {
            //шкалы из норм
            JArray normScales = new JArray();
            if (norm.Data["main"]["groups"]["item"]["quantities"]["item"] is JArray)
                foreach (var scale in norm.Data["main"]["groups"]["item"]["quantities"]["item"])
                    normScales.Add(scale);
            else
                normScales.Add(norm.Data["main"]["groups"]["item"]["quantities"]["item"]);


            //Для каждой вычисленной шкалы
            foreach (var result in results)
            {
                //если градация еще не определена
                if (result.gradationNumber == null)
                    foreach (var normScale in normScales)
                    {
                        //находим соответствующую шкалу из норм 
                        if (result.scores != null && result.idNormScale == normScale["id"].ToString())
                        {
                            //выбираем все градации шкалы
                            JArray grads = new JArray();
                            if (normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"] is JArray)
                                foreach (var grad in normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"])
                                    grads.Add(grad);
                            else
                                grads.Add(normScale["treelevel"]["children"]["item"]["treelevel"]["children"]["item"]["termexpr"]["gradations"]["gradations"]["item"]);



                            //Для каждой градации
                            foreach (var bsonGrad in grads)
                            {
                                var grad = JObject.Parse(bsonGrad.ToString());

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
                                    if (result.scores > double.Parse(grad["lowerformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture) &&
                                        result.scores <= double.Parse(grad["upperformula"]["ftext"].ToString(), System.Globalization.CultureInfo.InvariantCulture))
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

        private void CalculationByFormulas(CalcInfo test, List<Scale> results, Norm norm)
        {
            JArray scales = new JArray();
            if (test.Groups["item"] is JArray)
                foreach (var scale in test.Groups["item"])
                    scales.Add(scale);
            else
                scales.Add(test.Groups["item"]);

            //для каждой шкалы в результатах
            foreach (var result in results)
                //если баллов нет, будем вычислять
                if (result.scores == null)
                {
                    foreach (var scale in scales)
                        if (scale["ID"].ToString() == result.idTestScale)

                        {
                            if (scale["Formula"].ToString() == "ВЕСОВЫЕ КОЭФФИЦИЕНТЫ")
                                result.scores = 0;
                            else
                            {
                                string formula = scale["Formula"].ToString();
                                //парсим формулу
                                formula = ParseFormula.Parse(formula, results);

                                //расчет баллов по формуле
                                result.scores = Calculator.Calculate(formula);
                                //повторное нахождение диапазонов
                                RangeInterpretation(norm, patientResult.scales);
                            }
                            break;
                        }
                }
        }
    }
}
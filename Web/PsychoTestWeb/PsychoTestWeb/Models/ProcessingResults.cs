using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Xml;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;

namespace PsychoTestWeb.Models
{
    public class ProcessingResults
    {
        public PatientsResult patientResult = new PatientsResult();

        public ProcessingResults(JObject test, TestsResult result, BsonDocument norm)
        {
            //подсчет баллов по шкалам
            Dictionary<string, int> sum = Scorting(test, result, patientResult.scales);

            //добавление шкал пациенту
            AddScales(sum, test);

            //автоматическая интерпретация результатов
            //нахождение диапазонов
            RangeInterpretation(norm, patientResult.scales);
            //рассчет по формулам
            CalculationByFormulas(test, patientResult.scales, norm);
        }


        //подсчет баллов по шкалам
        private Dictionary<string, int> Scorting(JObject test, TestsResult result, List<PatientsResult.Scale> patientScales)
        {
            //id шкалы - сумма по шкале 
            Dictionary<string, int> scales = new Dictionary<string, int>();
            foreach (var answer in result.answers)
            {
                //Если вопрос с выбором одного из вариантов ответа
                if (Int32.Parse(test["Questions"]["item"][answer.question_id]["Question_Choice"].ToString()) == 1)
                {
                    if (answer.answer != "")
                        if (test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"] != null)
                            if (test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"].ToString() != "")
                            {
                                //id шкалы
                                string scale = "";
                                if (test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"]["item"] is JArray)
                                    foreach (var s in test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"]["item"])
                                    {
                                        scale = s["ScID"].ToString();
                                        if (scales.ContainsKey(scale))
                                            scales[scale] += Int32.Parse(s["Weights"].ToString());
                                        else
                                        {
                                            scales[scale] = Int32.Parse(s["Weights"].ToString());
                                        }
                                    }
                                else
                                {
                                    scale = test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"]["item"]["ScID"].ToString();
                                    if (scales.ContainsKey(scale))
                                        scales[scale] += Int32.Parse(test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"]["item"]["Weights"].ToString());
                                    else
                                    {
                                        scales[scale] = Int32.Parse(test["Questions"]["item"][answer.question_id]["Answers"]["item"][Int32.Parse(answer.answer)]["Weights"]["item"]["Weights"].ToString());
                                    }
                                }
                            }
                }
                //Если вопрос с вводом своего ответа
                else foreach (var ans in test["Questions"]["item"][answer.question_id]["Answers"]["item"])
                {
                    if (answer.answer == ans["Name"]["#text"].ToString())
                        if (ans["Weights"] != null)
                        {
                            if (ans["Weights"]["item"] is JArray)
                            {
                                foreach (var i in ans["Weights"]["item"])
                                    if (scales.ContainsKey(i["ScID"].ToString()))
                                        scales[i["ScID"].ToString()] += Int32.Parse(i["Weights"].ToString());
                                    else
                                    {
                                        scales[i["ScID"].ToString()] = Int32.Parse(i["Weights"].ToString());
                                    }
                            }
                            else
                            {
                                if (scales.ContainsKey(ans["Weights"]["item"]["ScID"].ToString()))
                                    scales[ans["Weights"]["item"]["ScID"].ToString()] += Int32.Parse(ans["Weights"]["item"]["Weights"].ToString());
                                else
                                {
                                    scales[ans["Weights"]["item"]["ScID"].ToString()] = Int32.Parse(ans["Weights"]["item"]["Weights"].ToString());
                                }
                            }
                        }
                }
            }
            return scales;
        }


        //добавление шкал пациенту
        private void AddScales (Dictionary<string, int> sum, JObject test)
        {
            if (test["Groups"]["item"] is JArray)
                foreach (var scale in test["Groups"]["item"])
                    if (sum.ContainsKey(scale["ID"].ToString()))
                        if (scale["NormID"] != null)
                            patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = scale["ID"].ToString(), idNormScale = scale["NormID"].ToString(), name = scale["Name"]["#text"].ToString(), scores = sum[scale["ID"].ToString()] });
                        else
                            patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = scale["ID"].ToString(), name = scale["Name"]["#text"].ToString(), scores = sum[scale["ID"].ToString()] });
                    else
                        if (scale["NormID"] != null)
                            patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = scale["ID"].ToString(), idNormScale = scale["NormID"].ToString(), name = scale["Name"]["#text"].ToString() });
                        else
                            patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = scale["ID"].ToString(), name = scale["Name"]["#text"].ToString() });
            else
            if (sum.ContainsKey(test["Groups"]["item"]["ID"].ToString()))
                if (test["Groups"]["item"]["NormID"] != null)
                    patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = test["Groups"]["item"]["ID"].ToString(), idNormScale = test["Groups"]["item"]["NormID"].ToString(), name = test["Groups"]["item"]["Name"]["#text"].ToString(), scores = sum[test["Groups"]["item"]["ID"].ToString()] });
                else
                    patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = test["Groups"]["item"]["ID"].ToString(), name = test["Groups"]["item"]["Name"]["#text"].ToString(), scores = sum[test["Groups"]["item"]["ID"].ToString()] });
            else
                if (test["Groups"]["item"]["NormID"] != null)
                    patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = test["Groups"]["item"]["ID"].ToString(), idNormScale = test["Groups"]["item"]["NormID"].ToString(), name = test["Groups"]["item"]["Name"]["#text"].ToString() });
                else
                    patientResult.scales.Add(new PatientsResult.Scale() { idTestScale = test["Groups"]["item"]["ID"].ToString(), name = test["Groups"]["item"]["Name"]["#text"].ToString() });
        }


        //автоматическая интерпретация результатов
        private void RangeInterpretation(BsonDocument norm, List<PatientsResult.Scale> results)
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
                                    if (result.scores <= Int32.Parse(grad["upperformula"]["ftext"].ToString()))
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
                                    if (result.scores > Int32.Parse(grad["lowerformula"]["ftext"].ToString()))
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
                                    if (result.scores > Int32.Parse(grad["lowerformula"]["ftext"].ToString()) && result.scores <= Int32.Parse(grad["upperformula"]["ftext"].ToString()))
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

        private void CalculationByFormulas (JObject test, List<PatientsResult.Scale> results, BsonDocument norm)
        {
            JArray scales = new JArray();
            if (test["Groups"]["item"] is JArray)
                foreach (var scale in test["Groups"]["item"])
                    scales.Add(scale);
            else
                scales.Add(test["Groups"]["item"]);

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
                                int firstIndex;
                                string formula = scale["Formula"].ToString();
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
                                            if (r.idTestScale == idTestScale)
                                            {
                                                value = (int)r.gradationNumber;
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
                                            if (r.idTestScale == idTestScale)
                                            {
                                                value = (int)r.scores;
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
                                            if (r.idTestScale == idTestScale)
                                            {
                                                value = (int)r.scores;
                                                break;
                                            }
                                        formula = formula.Remove(firstIndex, lastIndex - firstIndex + 1);
                                        formula = formula.Insert(firstIndex, value.ToString());
                                    }
                                } while (firstIndex != -1);

                                //расчет баллов по формуле
                                result.scores = (int)Calculator.Calculate(formula);
                                //повторное нахождение диапазонов
                                RangeInterpretation(norm, patientResult.scales);
                            }
                            break;
                        }
                }
        }

        //считает арифметическое вырежение, записанное в виде строки
        private static class Calculator
        {
            private const string numberChars = "01234567890.";
            private const string operatorChars = "^*/+-";

            public static double Calculate(string expression)
            {
                return EvaluateParenthesis(expression);
            }

            private static double EvaluateParenthesis(string expression)
            {
                string planarExpression = expression;
                while (planarExpression.Contains('('))
                {
                    int clauseStart = planarExpression.IndexOf('(') + 1;
                    int clauseEnd = IndexOfRightParenthesis(planarExpression, clauseStart);
                    string clause = planarExpression.Substring(clauseStart, clauseEnd - clauseStart);
                    planarExpression = planarExpression.Replace("(" + clause + ")", EvaluateParenthesis(clause).ToString());
                }
                return Evaluate(planarExpression);
            }

            private static int IndexOfRightParenthesis(string expression, int start)
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

            private static double Evaluate(string expression)
            {
                string normalExpression = expression.Replace(" ", "");
                List<char> operators = normalExpression.Split(numberChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToList();
                List<double> numbers = normalExpression.Split(operatorChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => double.Parse(x)).ToList();

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

            private static double Calc(double left, double right, char oper)
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

}

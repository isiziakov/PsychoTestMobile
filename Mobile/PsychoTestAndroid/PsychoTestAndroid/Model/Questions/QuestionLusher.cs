using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    public class QuestionLusher : QuestionText
    {
        public QuestionLusher(string id)
        {
            Id = id;
            text = "Из предложенных цветов выберите тот, который Вам больше всего нравится.";
            SetAnswers(null);
            Type = "Lusher";
        }

        public QuestionLusher(JObject data) : base(data)
        {

        }

        public override View Show(View layout)
        {
            LinearLayout answers = layout.FindViewById<LinearLayout>(Resource.Id.answers_view);
            var layouts = new List<LinearLayout>();
            for (int i = 0; i < 2; i++)
            {
                layouts.Add(new LinearLayout(answers.Context));
                answers.AddView(layouts.Last());
                layouts.Last().Orientation = 0;
                layouts.Last().LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
                layouts.Last().LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
                (layouts.Last().LayoutParameters as LinearLayout.LayoutParams).Weight = 1f;
            }
            
            for (int i = 0; i < 4; i++)
            {
                layouts[0] = Answers[i].Show(layouts[0]);
            }
            for (int i = 4; i < 8; i++)
            {
                layouts[1] = Answers[i].Show(layouts[1]);
            }
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            questionLinear.AddView(textView);
            return layout;
        }

        public override void SetAnswers(JObject data)
        {
            for (int i = 0; i < 8; i++)
            {
                Answers.Add(new AnswerLusher(i.ToString()));
                Answers.Last().owner = this;
            }
        }

        public override bool CheckResult()
        {
            foreach (Answer answer in Answers)
            {
                if (Result.IndexOf(answer.Id) == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
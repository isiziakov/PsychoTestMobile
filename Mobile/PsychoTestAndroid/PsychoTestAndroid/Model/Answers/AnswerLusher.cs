using Android.Graphics;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Answers
{
    public class AnswerLusher : Answer
    {
        Color color;
        ImageView image;
        public AnswerLusher(JObject data) : base(data)
        {
            
        }

        public AnswerLusher(string id)
        {
            Id = id;
            color = SetColor();
        }

        public override LinearLayout Show(LinearLayout layout)
        {
            image = new ImageView(layout.Context);
            image.SetBackgroundColor(color);
            layout.AddView(image);
            image.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            image.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
            (image.LayoutParameters as LinearLayout.LayoutParams).Weight = 1f;
            (image.LayoutParameters as LinearLayout.LayoutParams).SetMargins(8, 8, 8, 8);
            UpdateResult(Owner.Result);
            image.Click += (sender, e) =>
            {
                image.Clickable = false;
                image.Visibility = ViewStates.Invisible;
                if (Owner.Result == null || Owner.Result.Length == 0)
                {
                    Owner.Result = Id;
                }
                else
                {
                    Owner.Result += " " + Id;
                }
            };
            return base.Show(layout);
        }
        // Обновление ответа в соответствии с результатом вопроса.
        public override void UpdateResult(string result)
        {
            if (Owner.Result.IndexOf(Id) > -1)
            {
                image.Visibility = ViewStates.Invisible;
                image.Clickable = false;
            }
        }

        Color SetColor()
        {
            switch (Id)
            {
                case "0": return new Color(152,147,141, 255);
                case "1": return new Color(0,73,131, 255);
                case "2": return new Color(29,151,114, 255);
                case "3": return new Color(241,47,35, 255);
                case "4": return new Color(242,221,0, 255);
                case "5": return new Color(212,36,129,255);
                case "6": return new Color(197,82,35, 255);
                case "7": return new Color(35,31,32,255);
                default: return Color.White;  
            }
        }
    }
}
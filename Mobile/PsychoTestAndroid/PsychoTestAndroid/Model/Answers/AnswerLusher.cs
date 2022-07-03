using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            UpdateResult(owner.Result);
            image.Click += (sender, e) =>
            {
                image.Clickable = false;
                image.Visibility = ViewStates.Invisible;
                if (owner.Result == null || owner.Result.Length == 0)
                {
                    owner.Result = Id;
                }
                else
                {
                    owner.Result += " " + Id;
                }
            };
            return base.Show(layout);
        }
        // обновление ответа в соответствии с результатом вопроса
        public override void UpdateResult(string result)
        {
            if (owner.Result.IndexOf(Id) > -1)
            {
                image.Visibility = ViewStates.Invisible;
                image.Clickable = false;
            }
        }

        Color SetColor()
        {
            switch (Id)
            {
                case "0": return Color.Gray;
                case "1": return Color.Blue;
                case "2": return Color.Green;
                case "3": return Color.Red;
                case "4": return Color.Yellow;
                case "5": return Color.Violet;
                case "6": return Color.Brown;
                case "7": return Color.Black;
                default: return Color.White;  
            }
        }
    }
}
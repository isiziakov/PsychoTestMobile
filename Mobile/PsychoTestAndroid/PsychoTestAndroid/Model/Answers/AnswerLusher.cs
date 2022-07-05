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
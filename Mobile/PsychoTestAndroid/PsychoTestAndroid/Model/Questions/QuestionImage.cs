using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTestAndroid.Model.Questions
{
    // пока без картинок :)
    // вопрос с пояснением в виде картинки и текста
    public class QuestionImage : Question
    {
        // путь к картинке
        [JsonProperty("image")]
        string imageSrc;
        // текст с пояснением к картинке
        [JsonProperty("text")]
        string text;
        // bitmap с картинкой
        [JsonIgnore]
        Bitmap image;
        public QuestionImage()
        {
        }

        public QuestionImage(JObject data) : base(data)
        {
            imageSrc = data["Image"].ToString();
            imageSrc = imageSrc.Remove(0, imageSrc.IndexOf(",") + 1);
            byte[] imageData = Base64.Decode(imageSrc, Base64Flags.Default);
            image = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            text = data["Text"]["#text"].ToString();
        }
        // отрисовка вопроса
        public override View Show(View layout)
        {
            layout = base.Show(layout);
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            questionLinear.Orientation = Orientation.Vertical;
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            ImageView imageView = new ImageView(layout.Context);
            imageView.SetImageBitmap(image);
            questionLinear.AddView(textView);
            questionLinear.AddView(imageView);
            imageView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            imageView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            imageView.SetForegroundGravity(GravityFlags.Center);
            imageView.SetOnLongClickListener(new IncreaseImageOnLongClick(image));
            return layout;
        }
    }
}
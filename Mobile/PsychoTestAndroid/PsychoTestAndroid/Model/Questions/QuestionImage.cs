using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            imageSrc = data["ImageFileName"].ToString();
            text = data["Text"]["#text"].ToString();
        }
        // отрисовка вопроса
        public override View Show(View layout)
        {
            layout = base.Show(layout);
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            // если изображение еще не загружено
            if (image == null)
            { 
                image = getImage().GetAwaiter().GetResult();
            }
            questionLinear.AddView(textView);
            return layout;
        }
        // получить изображение
        private async Task<Bitmap> getImage()
        {
            return await Web.WebApi.GetImage(imageSrc);
        }
    }
}
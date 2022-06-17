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
using System.Threading.Tasks;

namespace PsychoTestAndroid.Model.Answers
{
    // декоратор с изображением
    public class ImageDecorator : AnswersDecorator
    {
        // путь к картинке
        string imageSrc;
        // картинка
        Bitmap image;
        public ImageDecorator()
        {

        }

        public ImageDecorator(JObject data)
        {
            imageSrc = data["ImageFileName"].ToString();
        }
        // отобразить декоратор
        public override LinearLayout Show(LinearLayout layout)
        {
            if (image == null)
            {
                image = getImage().GetAwaiter().GetResult();
            }
            TextView textView = new TextView(layout.Context);
            textView.Text = imageSrc;
            textView.TextSize = 20;
            layout.AddView(textView);
            textView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            textView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            return layout;
        }
        // получить изображение
        private async Task<Bitmap> getImage()
        {
            return await Web.WebApi.GetImage(imageSrc);
        }
    }
}
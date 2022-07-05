using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Helpers;
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
            imageSrc = data["Image"].ToString();
            imageSrc = imageSrc.Remove(0, imageSrc.IndexOf(",") + 1);
            byte[] imageData = Base64.Decode(imageSrc, Base64Flags.Default);
            image = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
        }
        // отобразить декоратор
        public override LinearLayout Show(LinearLayout layout)
        {
            ImageView imageView = new ImageView(layout.Context);
            imageView.SetImageBitmap(image);
            layout.AddView(imageView);
            imageView.SetOnLongClickListener(new IncreaseImageOnLongClick(image));
            imageView.Click += (sender, e) => { layout.CallOnClick(); };
            imageView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            imageView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            return layout;
        }
    }
}
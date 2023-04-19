using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Helpers;

namespace PsychoTestAndroid.Model.Answers
{
    // Декоратор с изображением.
    public class ImageDecorator : AnswersDecorator
    {
        // Путь к картинке.
        string imageSrc;
        // Картинка.
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
        // Отобразить декоратор.
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
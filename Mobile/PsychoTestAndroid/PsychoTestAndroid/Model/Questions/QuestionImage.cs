using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Helpers;

namespace PsychoTestAndroid.Model.Questions
{
    // Пока без картинок :)
    // Вопрос с пояснением в виде картинки и текста.
    public class QuestionImage : Question
    {
        // Путь к картинке.
        [JsonProperty("image")]
        string imageSrc;
        // Текст с пояснением к картинке.
        [JsonProperty("text")]
        string text;
        // Bitmap с картинкой.
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
        // Отрисовка вопроса.
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
            imageView.SetAdjustViewBounds(true);
            imageView.SetForegroundGravity(GravityFlags.Center);
            imageView.SetOnLongClickListener(new IncreaseImageOnLongClick(image));
            return layout;
        }
    }
}
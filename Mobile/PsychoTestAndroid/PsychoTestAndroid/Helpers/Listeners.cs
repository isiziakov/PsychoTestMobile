using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Helpers
{
    class IncreaseImageOnLongClick : Java.Lang.Object, View.IOnLongClickListener
    {
        Bitmap image;
        public IncreaseImageOnLongClick(Bitmap image) : base()
        {
            this.image = image;
        }
        public bool OnLongClick(View v)
        {
            Dialog builder = new Dialog(v.Context);
            builder.RequestWindowFeature((int)WindowFeatures.NoTitle);
            builder.Window.SetBackgroundDrawable(
                    new ColorDrawable(Color.Transparent));
            ImageView dImageView = new ImageView(v.Context);
            dImageView.SetImageBitmap(image);
            builder.AddContentView(dImageView, new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent));
            builder.Show();
            return true;
        }
    }
}
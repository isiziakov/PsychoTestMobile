using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Web
{
    public static class NotifyHelper
    {
        public static NotificationChannel channel { get; private set; }
        static string id;

        static NotifyHelper()
        {
            id = Application.Context.GetString(Resource.String.notification_channel_id);
            if (channel == null)
            {
                channel = CreateNotificationChannel();
            }
        }

        public static void ShowNewTestsNotification()
        {
            Intent resultIntent = new Intent(Application.Context, typeof(MainActivity));
            Android.App.TaskStackBuilder stackBuilder = Android.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.Immutable);

            var builder = new NotificationCompat.Builder(Application.Context, id)
              .SetAutoCancel(true)
              .SetContentIntent(resultPendingIntent)
              .SetContentTitle(Application.Context.GetString(Resource.String.notification_new_name))
              .SetSmallIcon(Resource.Drawable.abc_ic_star_black_16dp)
              .SetContentText(Application.Context.GetString(Resource.String.notification_new_description));

            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(Resource.String.notification_channel_id, builder.Build());
        }

        static NotificationChannel CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return null;
            }
            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            var channel = notificationManager.GetNotificationChannel(id);
            if (channel == null)
            {
                string name = Application.Context.GetString(Resource.String.notification_channel_name);
                channel = new NotificationChannel(id, name, NotificationImportance.Default)
                {
                    Description = Application.Context.GetString(Resource.String.notification_channel_descritption)
                };
                notificationManager.CreateNotificationChannel(channel);
            }
            return channel;
        }
    }
}
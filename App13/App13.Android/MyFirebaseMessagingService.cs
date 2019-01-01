using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Xamarin.Forms;
using static Android.Provider.Contacts;

namespace App13.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: "+message.From);

            var msg = message.Data["message"];
            string[] result = msg.Split("&&");
            string messag = result[0];
            string userName = result[1];
            string pass = result[2];
            string loadID = result[3];
            string userKey = result[4];
            //App.Current.Navigation.PushAsync(new Notification(msg));
            if (messag=="Rejected")
            {
                MainPage.refuse=true;
                App.Current.MainPage=new NavigationPage(new MainPage(userName, pass,loadID));
                //Start Notification
                String channelId = "Default";
                var intent = new Intent(this, typeof(MainActivity));
                var resultIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(Android.App.Application.Context.PackageName);
                resultIntent.SetAction(intent.Action);
                resultIntent.SetFlags(ActivityFlags.ClearTop|ActivityFlags.SingleTop);
                PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, requestCode: 0, intent: resultIntent, flags: PendingIntentFlags.UpdateCurrent|PendingIntentFlags.OneShot);
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Ваше предложение цены отклонено")
                .SetContentText("Откройте приложение чтобы продолжить")
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.abc_btn_radio_material);


                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                if (Build.VERSION.SdkInt>=Build.VERSION_CODES.O)
                {
                    NotificationChannel channel = new NotificationChannel(channelId, "Default channel", NotificationManager.ImportanceDefault);
                    manager.CreateNotificationChannel(channel);
                }

                manager.Notify(0, builder.Build());
            }
            else if (messag=="InTransit")
            {
                MainPage.refuse=false;
                MainPage.bid=false;
                MainPage.intransit=true;
                App.Current.MainPage=new NavigationPage(new MainPage(userName, pass,loadID));
                //Start Notification
                String channelId = "Default";
                var intent = new Intent(this, typeof(MainActivity));
                var resultIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(Android.App.Application.Context.PackageName);
                resultIntent.SetAction(intent.Action);
                resultIntent.SetFlags(ActivityFlags.ClearTop|ActivityFlags.SingleTop);
                PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, requestCode: 0, intent: resultIntent, flags: PendingIntentFlags.UpdateCurrent|PendingIntentFlags.OneShot);
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Заказ подтвержден")
                .SetContentText("Откройте приложение чтобы продолжить")
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.abc_btn_radio_material);


                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                if (Build.VERSION.SdkInt>=Build.VERSION_CODES.O)
                {
                    NotificationChannel channel = new NotificationChannel(channelId, "Default channel", NotificationManager.ImportanceDefault);
                    manager.CreateNotificationChannel(channel);
                }

                manager.Notify(0, builder.Build());

            }

            else
            {
                MainPage.intransit=false;
                App.Current.MainPage=new NavigationPage(new Notification(messag, userName, pass, userKey, loadID));

                //Log.Debug(TAG, "Notification Message Body: "+message.GetNotification().Body);

                //Start Notification
                String channelId = "Default";
                var intent = new Intent(this, typeof(MainActivity));
                var resultIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(Android.App.Application.Context.PackageName);
                resultIntent.SetAction(intent.Action);
                resultIntent.SetFlags(ActivityFlags.ClearTop|ActivityFlags.SingleTop);
                PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, requestCode: 0, intent: resultIntent, flags: PendingIntentFlags.UpdateCurrent|PendingIntentFlags.OneShot);
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Новый груз возле вас")
                .SetContentText("Откройте приложение чтобы продолжить")
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.abc_btn_radio_material);


                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                if (Build.VERSION.SdkInt>=Build.VERSION_CODES.O)
                {
                    NotificationChannel channel = new NotificationChannel(channelId, "Default channel", NotificationManager.ImportanceDefault);
                    manager.CreateNotificationChannel(channel);
                }

                manager.Notify(0, builder.Build());
            }
        }
    }

    
}
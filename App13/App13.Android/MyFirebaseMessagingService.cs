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
            App.Current.MainPage=new NavigationPage(new Notification(messag, userName, pass, userKey, loadID));

            //Log.Debug(TAG, "Notification Message Body: "+message.GetNotification().Body);
            String channelId = "Default";

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelId)
            .SetContentTitle("Новый груз возле вас")
            .SetContentText("Откройте приложение чтобы продолжить!")
            .SetSmallIcon(Resource.Drawable.abc_btn_radio_material);
            
            NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                NotificationChannel channel = new NotificationChannel(channelId, "Default channel", NotificationManager.ImportanceDefault);
                manager.CreateNotificationChannel(channel);
            }
            manager.Notify(0, builder.Build());
        }
    }

    
}
﻿using Admin.Activities;
using Android.App;
using Android.Content;
using Android.Media;
using AndroidX.Core.App;
using Firebase.Messaging;

namespace Admin.FirebaseHelper
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessagingModel : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            if (remoteMessage.GetNotification() != null)
            {
                SendNotification(remoteMessage);
            }
            base.OnMessageReceived(remoteMessage);
        }

        private void SendNotification(RemoteMessage remoteMessage)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this,"100")
                .SetSmallIcon(Resource.Drawable.app_icon)
                .SetContentTitle(remoteMessage.GetNotification().Title)
                .SetContentText(remoteMessage.GetNotification().Body)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);

            notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
        }
    }
}
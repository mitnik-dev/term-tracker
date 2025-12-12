using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace TermTracker.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class NotificationReceiver : BroadcastReceiver
    {
        private const string CHANNEL_ID = "TermTrackerChannel";

        public override void OnReceive(Context context, Intent intent)
        {
            var notificationId = intent.GetIntExtra("notificationId", 0);
            var title = intent.GetStringExtra("title") ?? "Term Tracker";
            var message = intent.GetStringExtra("message") ?? "";

            var notificationIntent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
            notificationIntent?.SetFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(
                context,
                notificationId,
                notificationIntent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            );

            var notification = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.notification_icon_background)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetPriority(NotificationCompat.PriorityDefault)
                .Build();

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(notificationId, notification);
        }
    }
}

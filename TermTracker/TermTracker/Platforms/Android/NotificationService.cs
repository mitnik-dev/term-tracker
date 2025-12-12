using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using TermTracker.Services;

namespace TermTracker.Platforms.Android
{
    public class NotificationService : INotificationService
    {
        private const string CHANNEL_ID = "TermTrackerChannel";
        private const string CHANNEL_NAME = "Term Tracker Notifications";

        public NotificationService()
        {
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default)
                {
                    Description = "Notifications for course start and end dates"
                };

                var notificationManager = (NotificationManager)Platform.CurrentActivity.GetSystemService(Context.NotificationService);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        public async Task<bool> RequestPermissionAsync()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
                return status == PermissionStatus.Granted;
            }
            return true;
        }

        public Task ScheduleNotificationAsync(int notificationId, string title, string message, DateTime notifyTime)
        {
            var intent = new Intent(Platform.CurrentActivity, typeof(NotificationReceiver));
            intent.PutExtra("notificationId", notificationId);
            intent.PutExtra("title", title);
            intent.PutExtra("message", message);

            var pendingIntent = PendingIntent.GetBroadcast(
                Platform.CurrentActivity,
                notificationId,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            );

            var alarmManager = (AlarmManager)Platform.CurrentActivity.GetSystemService(Context.AlarmService);
            var triggerTime = (long)(notifyTime.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                if (alarmManager?.CanScheduleExactAlarms() == true)
                {
                    alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
                }
                else
                {
                    alarmManager?.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
                }
            }
            else
            {
                alarmManager?.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }

            return Task.CompletedTask;
        }

        public void CancelNotification(int notificationId)
        {
            var intent = new Intent(Platform.CurrentActivity, typeof(NotificationReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(
                Platform.CurrentActivity,
                notificationId,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            );

            var alarmManager = (AlarmManager)Platform.CurrentActivity.GetSystemService(Context.AlarmService);
            alarmManager?.Cancel(pendingIntent);
            pendingIntent?.Cancel();
        }
    }
}

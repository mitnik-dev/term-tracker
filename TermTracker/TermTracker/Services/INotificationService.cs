namespace TermTracker.Services
{
    public interface INotificationService
    {
        Task<bool> RequestPermissionAsync();
        Task ScheduleNotificationAsync(int notificationId, string title, string message, DateTime notifyTime);
        void CancelNotification(int notificationId);
    }
}

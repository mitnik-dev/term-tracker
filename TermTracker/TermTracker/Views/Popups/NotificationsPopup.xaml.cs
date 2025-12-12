using CommunityToolkit.Maui.Views;
using TermTracker.Models;
using TermTracker.Services;

namespace TermTracker.Views.Popups;

public partial class NotificationsPopup : Popup
{
    private readonly Course _course;
    private readonly Term _term;
    private readonly INotificationService _notificationService;
    private readonly bool _isTermMode;

    public event EventHandler NotificationsSaved;

    public NotificationsPopup(Course course, INotificationService notificationService)
    {
        InitializeComponent();

        _course = course ?? throw new ArgumentNullException(nameof(course));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _isTermMode = false;

        StartDateLabel.Text = $"Start Date: {_course.StartDate:MM/dd/yyyy} at 9:00 AM";
        EndDateLabel.Text = $"End Date: {_course.EndDate:MM/dd/yyyy} at 9:00 AM";
    }

    public NotificationsPopup(Term term, INotificationService notificationService)
    {
        InitializeComponent();

        _term = term ?? throw new ArgumentNullException(nameof(term));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _isTermMode = true;

        StartDateLabel.Text = $"Start Date: {_term.StartDate:MM/dd/yyyy} at 9:00 AM";
        EndDateLabel.Text = $"End Date: {_term.EndDate:MM/dd/yyyy} at 9:00 AM";
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var hasPermission = await _notificationService.RequestPermissionAsync();

        if (!hasPermission)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Permission Denied",
                "Notification permission is required to schedule notifications.",
                "OK");
            return;
        }

        if (_isTermMode)
        {
            if (StartDateNotificationCheckBox.IsChecked)
            {
                await _notificationService.ScheduleNotificationAsync(
                    _term.Id * 1000 + 1,
                    "Term Starting Today",
                    $"{_term.Name} starts today!",
                    _term.StartDate.Date.AddHours(9)
                );
            }
            else
            {
                _notificationService.CancelNotification(_term.Id * 1000 + 1);
            }

            if (EndDateNotificationCheckBox.IsChecked)
            {
                await _notificationService.ScheduleNotificationAsync(
                    _term.Id * 1000 + 2,
                    "Term Ending Today",
                    $"{_term.Name} ends today!",
                    _term.EndDate.Date.AddHours(9)
                );
            }
            else
            {
                _notificationService.CancelNotification(_term.Id * 1000 + 2);
            }
        }
        else
        {
            if (StartDateNotificationCheckBox.IsChecked)
            {
                await _notificationService.ScheduleNotificationAsync(
                    _course.Id * 100 + 1,
                    "Course Starting Today",
                    $"{_course.Name} starts today!",
                    _course.StartDate.Date.AddHours(9)
                );
            }
            else
            {
                _notificationService.CancelNotification(_course.Id * 100 + 1);
            }

            if (EndDateNotificationCheckBox.IsChecked)
            {
                await _notificationService.ScheduleNotificationAsync(
                    _course.Id * 100 + 2,
                    "Course Ending Today",
                    $"{_course.Name} ends today!",
                    _course.EndDate.Date.AddHours(9)
                );
            }
            else
            {
                _notificationService.CancelNotification(_course.Id * 100 + 2);
            }
        }

        NotificationsSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }
}

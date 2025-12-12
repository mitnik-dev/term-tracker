using CommunityToolkit.Maui.Views;
using TermTracker.Models;
using TermTracker.Models.Enums;
using TermTracker.Services;

namespace TermTracker.Views.Popups;

public partial class AddAssessmentPopup : Popup
{
    private readonly int _courseId;
    private readonly INotificationService _notificationService;

    public event EventHandler<Assessment> AssessmentAdded;

    public AddAssessmentPopup(int courseId, INotificationService notificationService)
    {
        InitializeComponent();

        _courseId = courseId;
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        // Initialize with defaults
        TypePicker.SelectedIndex = 0; // Default to Objective
        StartDatePicker.Date = DateTime.Today;
        EndDatePicker.Date = DateTime.Today.AddDays(7);
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        if (TypePicker.SelectedIndex == -1)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Please select an assessment type.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Assessment name is required.", "OK");
            return;
        }

        if (EndDatePicker.Date < StartDatePicker.Date)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "End date must be after start date.", "OK");
            return;
        }

        var assessmentType = TypePicker.SelectedIndex == 0 ? AssessmentType.Objective : AssessmentType.Performance;

        var assessment = new Assessment
        {
            CourseId = _courseId,
            Name = NameEntry.Text,
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date,
            Type = assessmentType
        };

        // Schedule notifications if checked
        await ScheduleNotificationsAsync(assessment, StartDateNotificationCheckBox.IsChecked, EndDateNotificationCheckBox.IsChecked);

        // Raise event with the assessment
        AssessmentAdded?.Invoke(this, assessment);
        Close();
    }

    private async Task ScheduleNotificationsAsync(Assessment assessment, bool notifyStart, bool notifyEnd)
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

        if (notifyStart)
        {
            await _notificationService.ScheduleNotificationAsync(
                assessment.Id * 100 + (assessment.Type == AssessmentType.Objective ? 3 : 5),
                "Assessment Starting Today",
                $"{assessment.Name} starts today!",
                assessment.StartDate.Date.AddHours(9)
            );
        }

        if (notifyEnd)
        {
            await _notificationService.ScheduleNotificationAsync(
                assessment.Id * 100 + (assessment.Type == AssessmentType.Objective ? 4 : 6),
                "Assessment Ending Today",
                $"{assessment.Name} ends today!",
                assessment.EndDate.Date.AddHours(9)
            );
        }
    }
}

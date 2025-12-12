using CommunityToolkit.Maui.Views;
using TermTracker.Models;
using TermTracker.Services;

namespace TermTracker.Views.Popups;

public partial class EditAssessmentPopup : Popup
{
    private readonly Assessment _assessment;
    private readonly INotificationService _notificationService;

    public event EventHandler<Assessment> AssessmentSaved;
    public event EventHandler<Assessment> AssessmentDeleted;

    public EditAssessmentPopup(Assessment assessment, INotificationService notificationService)
    {
        InitializeComponent();

        _assessment = assessment ?? throw new ArgumentNullException(nameof(assessment));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        LoadAssessmentData();
    }

    private void LoadAssessmentData()
    {
        TypePicker.SelectedIndex = _assessment.Type == Models.Enums.AssessmentType.Objective ? 0 : 1;
        NameEntry.Text = _assessment.Name;
        StartDatePicker.Date = _assessment.StartDate;
        EndDatePicker.Date = _assessment.EndDate;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var result = await Application.Current.MainPage.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete '{_assessment.Name}'?",
            "Delete",
            "Cancel");

        if (result)
        {
            AssessmentDeleted?.Invoke(this, _assessment);
            Close();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
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

        _assessment.Type = TypePicker.SelectedIndex == 0 ? Models.Enums.AssessmentType.Objective : Models.Enums.AssessmentType.Performance;
        _assessment.Name = NameEntry.Text;
        _assessment.StartDate = StartDatePicker.Date;
        _assessment.EndDate = EndDatePicker.Date;

        await ScheduleNotificationsAsync(_assessment, StartDateNotificationCheckBox.IsChecked, EndDateNotificationCheckBox.IsChecked);

        AssessmentSaved?.Invoke(this, _assessment);
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
                assessment.Id * 100 + (assessment.Type == Models.Enums.AssessmentType.Objective ? 3 : 5),
                "Assessment Starting Today",
                $"{assessment.Name} starts today!",
                assessment.StartDate.Date.AddHours(9)
            );
        }

        if (notifyEnd)
        {
            await _notificationService.ScheduleNotificationAsync(
                assessment.Id * 100 + (assessment.Type == Models.Enums.AssessmentType.Objective ? 4 : 6),
                "Assessment Ending Today",
                $"{assessment.Name} ends today!",
                assessment.EndDate.Date.AddHours(9)
            );
        }
    }
}

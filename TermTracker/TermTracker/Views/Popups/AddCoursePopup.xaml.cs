using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Popups;

public partial class AddCoursePopup : Popup
{
    private readonly Course _newCourse;

    public event EventHandler<Course> CourseSaved;

    public AddCoursePopup(int termId)
    {
        InitializeComponent();

        _newCourse = new Course
        {
            Name = string.Empty,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(1),
            Status = StatusType.Future,
            InstructorName = string.Empty,
            InstructorPhone = string.Empty,
            InstructorEmail = string.Empty,
            TermId = termId
        };

        BindingContext = _newCourse;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_newCourse.Name))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Course name is required.", "OK");
            return;
        }

        if (_newCourse.EndDate < _newCourse.StartDate)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "End date must be after start date.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_newCourse.InstructorName))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Instructor name is required.", "OK");
            return;
        }

        if (!string.IsNullOrWhiteSpace(_newCourse.InstructorEmail) && !IsValidEmail(_newCourse.InstructorEmail))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Invalid email format.", "OK");
            return;
        }

        if (!string.IsNullOrWhiteSpace(_newCourse.InstructorPhone) && !IsValidPhone(_newCourse.InstructorPhone))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Invalid phone number format.", "OK");
            return;
        }

        CourseSaved?.Invoke(this, _newCourse);
        Close();
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private bool IsValidPhone(string phone)
    {
        var phonePattern = @"^\+?[\d\s\-\(\)]+$";
        var digitsOnly = Regex.Replace(phone, @"[^\d]", "");
        return Regex.IsMatch(phone, phonePattern) && digitsOnly.Length >= 10;
    }
}

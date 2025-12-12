using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Popups;

public partial class EditCourseDetailsPopup : Popup
{
    private readonly Course _editableCourse;

    public event EventHandler<Course> CourseSaved;
    public event EventHandler<int> CourseDeleted;

    public string Name
    {
        get => _editableCourse.Name;
        set => _editableCourse.Name = value;
    }

    public int StatusIndex
    {
        get => (int)_editableCourse.Status;
        set => _editableCourse.Status = (StatusType)value;
    }

    public DateTime StartDate
    {
        get => _editableCourse.StartDate;
        set => _editableCourse.StartDate = value;
    }

    public DateTime EndDate
    {
        get => _editableCourse.EndDate;
        set => _editableCourse.EndDate = value;
    }

    public string InstructorName
    {
        get => _editableCourse.InstructorName;
        set => _editableCourse.InstructorName = value;
    }

    public string InstructorPhone
    {
        get => _editableCourse.InstructorPhone;
        set => _editableCourse.InstructorPhone = value;
    }

    public string InstructorEmail
    {
        get => _editableCourse.InstructorEmail;
        set => _editableCourse.InstructorEmail = value;
    }

    public EditCourseDetailsPopup(Course course)
    {
        InitializeComponent();

        _editableCourse = new Course
        {
            Id = course.Id,
            Name = course.Name,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            Status = course.Status,
            InstructorName = course.InstructorName,
            InstructorPhone = course.InstructorPhone,
            InstructorEmail = course.InstructorEmail,
            TermId = course.TermId
        };

        StatusPicker.ItemsSource = Enum.GetNames(typeof(StatusType)).ToList();

        BindingContext = this;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_editableCourse.Name))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Course name is required.", "OK");
            return;
        }

        if (_editableCourse.EndDate < _editableCourse.StartDate)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "End date must be after start date.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_editableCourse.InstructorName))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Instructor name is required.", "OK");
            return;
        }

        if (!string.IsNullOrWhiteSpace(_editableCourse.InstructorEmail) && !IsValidEmail(_editableCourse.InstructorEmail))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Invalid email format.", "OK");
            return;
        }

        if (!string.IsNullOrWhiteSpace(_editableCourse.InstructorPhone) && !IsValidPhone(_editableCourse.InstructorPhone))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Invalid phone number format.", "OK");
            return;
        }

        CourseSaved?.Invoke(this, _editableCourse);
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

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Course",
            "Are you sure you want to delete this course? This will remove all related assessments and notes.",
            "Delete",
            "Cancel");

        if (confirm)
        {
            CourseDeleted?.Invoke(this, _editableCourse.Id);
            Close();
        }
    }
}

using CommunityToolkit.Maui.Views;
using TermTracker.Models;

namespace TermTracker.Views.Popups;

public partial class EditCoursePopup : Popup
{
    private readonly Course _editableCourse;

    public event EventHandler<Course> CourseSaved;
    public event EventHandler<int> CourseDeleted;

    public EditCoursePopup(Course course)
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

        BindingContext = _editableCourse;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_editableCourse.EndDate < _editableCourse.StartDate)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "End date must be after start date.", "OK");
            return;
        }

        CourseSaved?.Invoke(this, _editableCourse);
        Close();
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

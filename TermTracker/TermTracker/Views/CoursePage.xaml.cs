using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using TermTracker.Models;
using TermTracker.Services;
using TermTracker.Views.Popups;

namespace TermTracker.Views;

public partial class CoursePage : ContentPage
{
    public ObservableCollection<Assessment> Assessments { get; set; }
    public ObservableCollection<Note> Notes { get; set; }

    private Course _course;
    private readonly DatabaseService _databaseService = new();
    private readonly INotificationService _notificationService;

    private bool _showAddAssessmentButton = true;
    public bool ShowAddAssessmentButton
    {
        get => _showAddAssessmentButton;
        set
        {
            _showAddAssessmentButton = value;
            OnPropertyChanged(nameof(ShowAddAssessmentButton));
        }
    }

    public CoursePage(Course course)
    {
        Assessments = new ObservableCollection<Assessment>();
        Notes = new ObservableCollection<Note>();
        _course = course;

#if ANDROID
        _notificationService = new Platforms.Android.NotificationService();
#else
        _notificationService = null;
#endif

        InitializeComponent();
        BindingContext = this;
        LoadData();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshCourseDataAsync();
    }

    private async Task RefreshCourseDataAsync()
    {
        var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
        if (refreshedCourse != null)
        {
            _course.Name = refreshedCourse.Name;
            _course.Status = refreshedCourse.Status;
            _course.StartDate = refreshedCourse.StartDate;
            _course.EndDate = refreshedCourse.EndDate;
            _course.InstructorName = refreshedCourse.InstructorName;
            _course.InstructorPhone = refreshedCourse.InstructorPhone;
            _course.InstructorEmail = refreshedCourse.InstructorEmail;
            _course.Assessments = refreshedCourse.Assessments;
            _course.Notes = refreshedCourse.Notes;
            LoadData();
        }
    }

    private void LoadData()
    {
        CourseStatusLabel.Text = _course.Status.ToString();
        CourseNameLabel.Text = _course.Name;
        DateRange.Text = $"{_course.StartDate:MM/dd/yy} - {_course.EndDate:MM/dd/yy}";

        InstructorNameLabel.Text = _course.InstructorName;
        InstructorPhoneLabel.Text = _course.InstructorPhone;
        InstructorEmailLabel.Text = _course.InstructorEmail;

        Assessments.Clear();
        Notes.Clear();

        if (_course.Assessments != null)
        {
            foreach (var assessment in _course.Assessments)
            {
                Assessments.Add(assessment);
            }
        }

        if (_course.Notes != null)
        {
            foreach (var note in _course.Notes)
            {
                Notes.Add(note);
            }
        }

        NotesGridControl?.ForceUpdate();

        // Always show the + button
        ShowAddAssessmentButton = true;
    }

    private async void OnNotificationsClicked(object sender, EventArgs e)
    {
        if (_notificationService == null)
        {
            await DisplayAlert("Error", "Notification service is not available on this platform.", "OK");
            return;
        }

        var popup = new NotificationsPopup(_course, _notificationService);
        await this.ShowPopupAsync(popup);
    }

    private async void OnEditCourseClicked(object sender, EventArgs e)
    {
        var popup = new EditCourseDetailsPopup(_course);

        popup.CourseSaved += async (s, updatedCourse) =>
        {
            await _databaseService.UpdateCourseAsync(updatedCourse);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(updatedCourse.Id) ?? updatedCourse;
            _course.Name = refreshedCourse.Name;
            _course.Status = refreshedCourse.Status;
            _course.StartDate = refreshedCourse.StartDate;
            _course.EndDate = refreshedCourse.EndDate;
            _course.InstructorName = refreshedCourse.InstructorName;
            _course.InstructorPhone = refreshedCourse.InstructorPhone;
            _course.InstructorEmail = refreshedCourse.InstructorEmail;
            _course.Assessments = refreshedCourse.Assessments;
            _course.Notes = refreshedCourse.Notes;
            LoadData();
        };

        popup.CourseDeleted += async (s, courseId) =>
        {
            await _databaseService.DeleteCourseAsync(courseId);
            await Navigation.PopAsync();
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnAddAssessmentClicked(object sender, EventArgs e)
    {
        if (_notificationService == null)
        {
            await DisplayAlert("Error", "Notification service is not available on this platform.", "OK");
            return;
        }


        if (_course.Assessments != null && _course.Assessments.Count >= 2)
        {
            await DisplayAlert("Maximum Assessments", "This course already has the maximum of 2 assessments.", "OK");
            return;
        }

        var popup = new AddAssessmentPopup(_course.Id, _notificationService);

        popup.AssessmentAdded += async (s, assessment) =>
        {
            await _databaseService.AddAssessmentAsync(assessment);

            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Assessments = refreshedCourse.Assessments;
                Assessments.Clear();
                if (_course.Assessments != null)
                {
                    foreach (var assess in _course.Assessments)
                        Assessments.Add(assess);
                }

            }
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnAddNoteClicked(object sender, EventArgs e)
    {
        var popup = new AddNotePopup(_course.Id);

        popup.NoteSaved += async (s, newNote) =>
        {
            await _databaseService.AddNoteAsync(newNote);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Notes = refreshedCourse.Notes;
                Notes.Clear();
                if (_course.Notes != null)
                {
                    foreach (var note in _course.Notes)
                        Notes.Add(note);
                }
                if (NotesGridControl != null)
                {
                    NotesGridControl.Notes = null;
                    NotesGridControl.Notes = Notes;
                    NotesGridControl.ForceUpdate();
                }
            }
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnNoteClicked(object sender, Note note)
    {
        var popup = new EditNotePopup(note);

        popup.NoteSaved += async (s, updatedNote) =>
        {
            await _databaseService.UpdateNoteAsync(updatedNote);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Notes = refreshedCourse.Notes;
                Notes.Clear();
                if (_course.Notes != null)
                {
                    foreach (var note in _course.Notes)
                        Notes.Add(note);
                }
                if (NotesGridControl != null)
                {
                    NotesGridControl.Notes = null;
                    NotesGridControl.Notes = Notes;
                    NotesGridControl.ForceUpdate();
                }
            }
        };

        popup.NoteDeleted += async (s, deletedNote) =>
        {
            await _databaseService.DeleteNoteAsync(deletedNote.Id);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Notes = refreshedCourse.Notes;
                Notes.Clear();
                if (_course.Notes != null)
                {
                    foreach (var note in _course.Notes)
                        Notes.Add(note);
                }
                if (NotesGridControl != null)
                {
                    NotesGridControl.Notes = null;
                    NotesGridControl.Notes = Notes;
                    NotesGridControl.ForceUpdate();
                }
            }
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnAssessmentClicked(object sender, Assessment assessment)
    {
        if (_notificationService == null)
        {
            await DisplayAlert("Error", "Notification service is not available on this platform.", "OK");
            return;
        }

        var popup = new EditAssessmentPopup(assessment, _notificationService);

        popup.AssessmentSaved += async (s, updatedAssessment) =>
        {
            await _databaseService.UpdateAssessmentAsync(updatedAssessment);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Assessments = refreshedCourse.Assessments;
                Assessments.Clear();
                if (_course.Assessments != null)
                {
                    foreach (var assess in _course.Assessments)
                        Assessments.Add(assess);
                }
                // The + button is always visible
            }
        };

        popup.AssessmentDeleted += async (s, deletedAssessment) =>
        {
            await _databaseService.DeleteAssessmentAsync(deletedAssessment.Id);
            var refreshedCourse = await _databaseService.GetCourseByIdAsync(_course.Id);
            if (refreshedCourse != null)
            {
                _course.Assessments = refreshedCourse.Assessments;
                Assessments.Clear();
                if (_course.Assessments != null)
                {
                    foreach (var assess in _course.Assessments)
                        Assessments.Add(assess);
                }

                // The + button is always visible
            }
        };

        await this.ShowPopupAsync(popup);
    }
}

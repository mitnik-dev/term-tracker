using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using TermTracker.Models;
using TermTracker.Services;
using TermTracker.Views.Popups;

namespace TermTracker.Views;

public partial class TermPage : ContentPage
{
    public ObservableCollection<Course> OtherCourses { get; set; }

    public ObservableCollection<Course> CurrentCourses { get; set; }
    private Term _term;
    private readonly DatabaseService _databaseService = new();
    private readonly INotificationService _notificationService;
    private List<Course> _allOtherCourses = new List<Course>();

    public TermPage(Term term)
    {
        InitializeComponent();

        CurrentCourses = new ObservableCollection<Course>();
        OtherCourses = new ObservableCollection<Course>();

#if ANDROID
        _notificationService = new Platforms.Android.NotificationService();
#else
        _notificationService = null;
#endif

        BindingContext = this;
        _term = term;
        LoadData();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshTermDataAsync();
    }

    private async Task RefreshTermDataAsync()
    {
        var refreshedTerm = await _databaseService.GetTermByIdAsync(_term.Id);
        if (refreshedTerm != null)
        {
            _term.Name = refreshedTerm.Name;
            _term.StartDate = refreshedTerm.StartDate;
            _term.EndDate = refreshedTerm.EndDate;
            _term.Courses = refreshedTerm.Courses;
            LoadData();
        }
    }

    private void LoadData()
    {
        TermStatusLabel.Text = _term.Status.ToString();
        TermNameLabel.Text = _term.Name;
        DateRange.Text = $"{_term.StartDate:MM/dd/yy} - {_term.EndDate:MM/dd/yy}";

        OtherCourses.Clear();
        CurrentCourses.Clear();
        _allOtherCourses.Clear();

        foreach (var course in _term.Courses)
        {
            if (DateTime.Now > course.StartDate && DateTime.Now < course.EndDate)
            {
                CurrentCourses.Add(course);
            }
            else
            {
                OtherCourses.Add(course);
                _allOtherCourses.Add(course);
            }
        }

        if (CurrentCourses.Count == 0)
        {
            CurrentCoursesContainer.IsVisible = false;
            NoActiveCoursesMessage.IsVisible = true;
        }
        else
        {
            CurrentCoursesContainer.IsVisible = true;
            NoActiveCoursesMessage.IsVisible = false;
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        OtherCourses.Clear();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            foreach (var course in _allOtherCourses)
                OtherCourses.Add(course);
        }
        else
        {
            var filteredCourses = _allOtherCourses.Where(c =>
                c.Name.ToLower().Contains(searchText) ||
                c.InstructorName.ToLower().Contains(searchText) ||
                c.InstructorEmail.ToLower().Contains(searchText) ||
                c.InstructorPhone.ToLower().Contains(searchText) ||
                c.StartDate.ToString("MM/dd/yyyy").Contains(searchText) ||
                c.EndDate.ToString("MM/dd/yyyy").Contains(searchText)
            ).ToList();

            foreach (var course in filteredCourses)
                OtherCourses.Add(course);
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var popup = new AddCoursePopup(_term.Id);

        popup.CourseSaved += async (s, newCourse) =>
        {
            await _databaseService.AddCourseAsync(newCourse);
            var refreshedTerm = await _databaseService.GetTermByIdAsync(_term.Id);
            _term.Courses = refreshedTerm.Courses;
            LoadData();
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnCourseCardClicked(object sender, object e)
    {
        if (e is Course course)
        {
            await Navigation.PushAsync(new CoursePage(course));
        }
    }

    private async void OnNotificationsClicked(object sender, EventArgs e)
    {
        if (_notificationService == null)
        {
            await DisplayAlert("Error", "Notification service is not available on this platform.", "OK");
            return;
        }

        var popup = new NotificationsPopup(_term, _notificationService);
        await this.ShowPopupAsync(popup);
    }

    private async void OnEditTermClicked(object sender, EventArgs e)
    {
        var popup = new EditTermPopup(_term);

        popup.TermSaved += async (s, updatedTerm) =>
        {
            await _databaseService.UpdateTermAsync(updatedTerm);
            var refreshedTerm = await _databaseService.GetTermByIdAsync(updatedTerm.Id) ?? updatedTerm;
            _term.Name = refreshedTerm.Name;
            _term.StartDate = refreshedTerm.StartDate;
            _term.EndDate = refreshedTerm.EndDate;
            _term.Courses = refreshedTerm.Courses;
            LoadData();
        };

        popup.TermDeleted += async (s, termId) =>
        {
            await _databaseService.DeleteTermAsync(termId);
            await Navigation.PopAsync();
        };

        await this.ShowPopupAsync(popup);
    }

}
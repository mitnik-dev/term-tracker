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

    public TermPage(Term term)
    {
        InitializeComponent();

        CurrentCourses = new ObservableCollection<Course>();
        OtherCourses = new ObservableCollection<Course>();

        BindingContext = this;
        _term = term;
        LoadData();
    }

    private void LoadData()
    {
        TermNameLabel.Text = _term.Name;
        DateRange.Text = $"{_term.StartDate:MM/dd/yy} - {_term.EndDate:MM/dd/yy}";

        OtherCourses.Clear();
        CurrentCourses.Clear();

        foreach (var course in _term.Courses)
        {
            if (DateTime.Now > course.StartDate && DateTime.Now < course.EndDate)
            {
                CurrentCourses.Add(course);
            }
            else
            {
                OtherCourses.Add(course);
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

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // Handle add term button click
        // You can navigate to an add term page or show a dialog
        await DisplayAlert("Add Term", "Add term functionality coming soon", "OK");
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
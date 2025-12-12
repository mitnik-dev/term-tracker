using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using TermTracker.Models;
using TermTracker.Services;
using TermTracker.Views;
using TermTracker.Views.Popups;

namespace TermTracker;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Term> Terms { get; set; }
    private List<Term> _allTerms = new List<Term>();

    public MainPage()
    {
        InitializeComponent();
        Terms = new ObservableCollection<Term>();
        _databaseService = new DatabaseService();

        DateLabel.Text = $"{DateTime.Now:dddd, MMM. d}";

        _ = LoadDataAsync();
        BindingContext = this;

    }

    private async void OnCardClicked(object sender, object e)
    {
        if (e is Term term)
        {
            await Navigation.PushAsync(new TermPage(term));
        }
        else if (e is Course course)
        {
            await Navigation.PushAsync(new Views.CoursePage(course));
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var popup = new AddTermPopup();

        popup.TermSaved += async (s, newTerm) =>
        {
            await _databaseService.AddTermAsync(newTerm);
            var insertedTerm = await _databaseService.GetTermByIdAsync(newTerm.Id) ?? newTerm;
            await LoadDataAsync();
            await Navigation.PushAsync(new TermPage(insertedTerm));
        };

        await this.ShowPopupAsync(popup);
    }

    private async void OnReportsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReportsPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }
    private async Task LoadDataAsync()
    {
        var allTerms = await _databaseService.GetAllTermsAsync();
        var currentTerm = await _databaseService.GetCurrentTermAsync();

        if (currentTerm == null)
        {
            CurrentTermCard.Term = new Term();
            CurrentTermCard.Term = null;

            _allTerms = allTerms.ToList();
            Terms.Clear();
            foreach (var term in allTerms)
                Terms.Add(term);
        }
        else
        {
            CurrentTermCard.Term = currentTerm;

            _allTerms = allTerms.Where(t => t.Id != currentTerm.Id).ToList();
            Terms.Clear();
            foreach (var term in allTerms)
            {
                if (currentTerm.Id == term.Id)
                    continue;

                Terms.Add(term);
            }
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        Terms.Clear();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            foreach (var term in _allTerms)
                Terms.Add(term);
        }
        else
        {
            var filteredTerms = _allTerms.Where(t =>
                t.Name.ToLower().Contains(searchText) ||
                t.StartDate.ToString("MM/dd/yyyy").Contains(searchText) ||
                t.EndDate.ToString("MM/dd/yyyy").Contains(searchText)
            ).ToList();

            foreach (var term in filteredTerms)
                Terms.Add(term);
        }
    }


    private double CalculateProgress(DateTime startDate, DateTime endDate)
    {
        DateTime now = DateTime.Now;

        if (now < startDate)
            return 0.0;

        if (now > endDate)
            return 100.0;

        TimeSpan totalDuration = endDate - startDate;
        TimeSpan elapsedDuration = now - startDate;

        if (totalDuration.TotalDays <= 0)
            return 100.0;

        double progress = (elapsedDuration.TotalDays / totalDuration.TotalDays) * 100.0;
        return Math.Clamp(progress, 0.0, 100.0);
    }

    //private static void UpdateProgressBar()
    //{
    //    var assessments = await _databaseService.GetAllAssessmentsAsync();

    //}
}

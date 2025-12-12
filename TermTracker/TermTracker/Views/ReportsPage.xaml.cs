using TermTracker.Models;
using TermTracker.Models.Enums;
using TermTracker.Services;

namespace TermTracker.Views;

public partial class ReportsPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private List<Term> _allTerms;

    public ReportsPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _ = LoadTermsAsync();
    }

    private async Task LoadTermsAsync()
    {
        _allTerms = await _databaseService.GetAllTermsAsync();
        TermPicker.ItemsSource = _allTerms.Select(t => t.Name).ToList();
    }

    private async void OnTermSelected(object sender, EventArgs e)
    {
        if (TermPicker.SelectedIndex < 0) return;

        var selectedTerm = _allTerms[TermPicker.SelectedIndex];
        await GenerateReportAsync(selectedTerm.Id);
    }

    private async Task GenerateReportAsync(int termId)
    {
        var term = await _databaseService.GetTermByIdAsync(termId);
        if (term == null) return;

        ReportContainer.IsVisible = true;

        ReportTitle.Text = $"Academic Term Report - {term.Name}";
        TermDateRange.Text = $"{term.StartDate:MM/dd/yyyy} - {term.EndDate:MM/dd/yyyy}";

        CoursesContainer.Children.Clear();

        int totalAssessments = 0;

        foreach (var course in term.Courses)
        {
            var courseCard = CreateCourseCard(course);
            CoursesContainer.Children.Add(courseCard);
            totalAssessments += course.Assessments.Count;
        }

        TotalCoursesLabel.Text = $"Total Courses: {term.Courses.Count}";
        TotalAssessmentsLabel.Text = $"Total Assessments: {totalAssessments}";
    }

    private Frame CreateCourseCard(Course course)
    {
        var frame = new Frame
        {
            BackgroundColor = (Color)Application.Current.Resources["BgLight"],
            BorderColor = (Color)Application.Current.Resources["BorderMuted"],
            CornerRadius = 12,
            Padding = 16,
            HasShadow = false
        };

        var mainGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            RowSpacing = 6,
            ColumnSpacing = 12
        };

        var courseTitle = new Label
        {
            Text = course.Name,
            FontFamily = "InterMedium18",
            FontSize = 16,
            TextColor = (Color)Application.Current.Resources["Text"],
            LineBreakMode = LineBreakMode.WordWrap,
            Margin = new Thickness(0, 0, 0, 8)
        };
        mainGrid.Add(courseTitle, 0, 0);
        Grid.SetColumnSpan(courseTitle, 2);

        AddGridLabel(mainGrid, "Status:", 1, 0, true);
        AddGridLabel(mainGrid, course.Status.ToString(), 1, 1, false);


        AddGridLabel(mainGrid, "Start Date:", 2, 0, true);
        AddGridLabel(mainGrid, course.StartDate.ToString("MM/dd/yyyy"), 2, 1, false);


        AddGridLabel(mainGrid, "End Date:", 3, 0, true);
        AddGridLabel(mainGrid, course.EndDate.ToString("MM/dd/yyyy"), 3, 1, false);


        AddGridLabel(mainGrid, "Instructor:", 4, 0, true);
        AddGridLabel(mainGrid, course.InstructorName, 4, 1, false);


        if (course.Assessments.Any())
        {
            var assessmentHeader = new Label
            {
                Text = "Assessments:",
                Style = (Style)Application.Current.Resources["DefaultText"],
                Margin = new Thickness(0, 8, 0, 4)
            };
            mainGrid.Add(assessmentHeader, 0, 5);
            Grid.SetColumnSpan(assessmentHeader, 2);


            var assessmentsGrid = CreateAssessmentsGrid(course.Assessments);
            mainGrid.Add(assessmentsGrid, 0, 6);
            Grid.SetColumnSpan(assessmentsGrid, 2);
        }

        frame.Content = mainGrid;
        return frame;
    }

    private Grid CreateAssessmentsGrid(List<Assessment> assessments)
    {
        var assessmentsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(90, GridUnitType.Absolute) }
            },
            RowSpacing = 8,
            ColumnSpacing = 10
        };

        var typeHeader = new Label
        {
            Text = "Type",
            Style = (Style)Application.Current.Resources["MutedText"]
        };
        assessmentsGrid.Add(typeHeader, 0, 0);

        var nameHeader = new Label
        {
            Text = "Name",
            Style = (Style)Application.Current.Resources["MutedText"]
        };
        assessmentsGrid.Add(nameHeader, 1, 0);

        var dueHeader = new Label
        {
            Text = "Due Date",
            Style = (Style)Application.Current.Resources["MutedText"]
        };
        assessmentsGrid.Add(dueHeader, 2, 0);

        assessmentsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        int rowIndex = 1;
        foreach (var assessment in assessments)
        {
            assessmentsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var typeBadge = new Frame
            {
                BackgroundColor = assessment.Type == AssessmentType.Objective
                    ? (Color)Application.Current.Resources["Info"]
                    : (Color)Application.Current.Resources["Success"],
                CornerRadius = 12,
                Padding = new Thickness(6, 3),
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            typeBadge.Content = new Label
            {
                Text = assessment.Type.ToString(),
                FontSize = 11,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            assessmentsGrid.Add(typeBadge, 0, rowIndex);

            var nameLabel = new Label
            {
                Text = assessment.Name,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.Center
            };
            assessmentsGrid.Add(nameLabel, 1, rowIndex);

            var dueDateLabel = new Label
            {
                Text = assessment.EndDate.ToString("MM/dd/yyyy"),
                VerticalOptions = LayoutOptions.Center
            };
            assessmentsGrid.Add(dueDateLabel, 2, rowIndex);

            rowIndex++;
        }

        return assessmentsGrid;
    }

    private void AddGridLabel(Grid grid, string text, int row, int column, bool isBold)
    {
        var label = new Label
        {
            Text = text,
            LineBreakMode = LineBreakMode.WordWrap,
            VerticalOptions = LayoutOptions.Center
        };

        if (isBold)
        {
            label.Style = (Style)Application.Current.Resources["DefaultText"];
        }

        grid.Add(label, column, row);
    }
}

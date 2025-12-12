using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Components;

public partial class CardDetailed : ContentView
{
    public static readonly BindableProperty AcademicTypeProperty =
        BindableProperty.Create(
            nameof(AcademicType),
            typeof(AcademicType),
            typeof(CardDetailed),
            AcademicType.Term,
            propertyChanged: OnAcademicTypeChanged,
            validateValue: ValidateAcademicType);

    public event EventHandler<object> CardClicked;

    public AcademicType AcademicType
    {
        get => (AcademicType)GetValue(AcademicTypeProperty);
        set => SetValue(AcademicTypeProperty, value);
    }

    private static bool ValidateAcademicType(BindableObject bindable, object value)
    {
        if (value is AcademicType type)
        {
            return type == AcademicType.Term || type == AcademicType.Courses;
        }
        return false;
    }

    public static readonly BindableProperty TermProperty =
        BindableProperty.Create(
            nameof(Term),
            typeof(Term),
            typeof(CardDetailed),
            null,
            propertyChanged: OnModelChanged);

    public Term Term
    {
        get => (Term)GetValue(TermProperty);
        set => SetValue(TermProperty, value);
    }

    public static readonly BindableProperty CourseProperty =
        BindableProperty.Create(
            nameof(Course),
            typeof(Course),
            typeof(CardDetailed),
            null,
            propertyChanged: OnModelChanged);

    public Course Course
    {
        get => (Course)GetValue(CourseProperty);
        set => SetValue(CourseProperty, value);
    }

    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(
            nameof(Progress),
            typeof(double),
            typeof(CardDetailed),
            0.0,
            propertyChanged: OnProgressChanged);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public CardDetailed()
    {
        InitializeComponent();
    }

    private async void OnFrameTapped(object sender, TappedEventArgs e)
    {
        if (Term != null && AcademicType == AcademicType.Term)
        {
            await Navigation.PushAsync(new TermPage(Term));
        }
        else if (Course != null && AcademicType == AcademicType.Courses)
        {
            await Navigation.PushAsync(new CoursePage(Course));
        }
    }

    private static void OnAcademicTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardDetailed control)
        {
            control.UpdateView();
        }
    }

    private static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardDetailed control)
        {
            control.UpdateView();
        }
    }

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardDetailed control && control.ProgressBar != null)
        {
            control.ProgressBar.Progress = (double)newValue;
        }
    }

    private void UpdateView()
    {
        if ((AcademicType == AcademicType.Term && Term == null) ||
       (AcademicType == AcademicType.Courses && Course == null))
        {
            NoActiveLabel.IsVisible = true;
            AcademicGrid.IsVisible = false;

            NoActiveLabel.Text = $"No active {AcademicType.ToString().ToLower()}";

            return;
        }
        NoActiveLabel.IsVisible = false;
        AcademicGrid.IsVisible = true;

        if (AcademicType == AcademicType.Term && Term != null)
        {
            UpdateFromTerm(Term);
        }
        else if (AcademicType == AcademicType.Courses && Course != null)
        {
            UpdateFromCourse(Course);
        }

        if (ProgressBar != null)
        {
            ProgressBar.AcademicType = AcademicType;
            ProgressBar.Progress = Progress;
        }
    }

    private void UpdateFromTerm(Term term)
    {
        if (TitleLabel != null)
        {
            TitleLabel.Text = term.Name ?? string.Empty;
        }

        if (DateRange != null)
        {
            DateRange.Text = $"{term.StartDate:MM/dd/yy} - {term.EndDate:MM/dd/yy}";
        }

        if (StatusIndicator != null)
        {
            StatusIndicator.Status = term.Status;
        }

        if (ButtonIndicator != null)
        {
            ButtonIndicator.Status = term.Status;
        }

        if (NoOfChildren != null)
        {
            int courseCount = term.Courses?.Count ?? 0;
            NoOfChildren.Text = courseCount == 1
                ? "1 Course"
                : $"{courseCount} Courses";
        }

        if (InfoLabel1 != null && InfoLabel2 != null)
        {
            string season = GetSeason(term.StartDate);
            int weeksLeft = GetWeeksUntil(DateTime.Now, term.EndDate);
            InfoLabel1.Text = $"{season}";
            InfoLabel2.Text = $"{weeksLeft} Wks. Left";
        }

        double progress = CalculateProgress(term.StartDate, term.EndDate);
        Progress = progress;
        if (ProgressBar != null)
        {
            ProgressBar.Progress = progress;
        }
    }

    private void UpdateFromCourse(Course course)
    {
        if (TitleLabel != null)
        {
            TitleLabel.Text = course.Name ?? string.Empty;
        }

        if (DateRange != null)
        {
            DateRange.Text = $"{course.StartDate:MM/dd/yy} - {course.EndDate:MM/dd/yy}";
        }

        if (StatusIndicator != null)
        {
            StatusIndicator.Status = course.Status;
        }

        if (ButtonIndicator != null)
        {
            ButtonIndicator.Status = course.Status;
        }

        if (NoOfChildren != null)
        {
            int assessmentCount = course.Assessments?.Count ?? 0;
            NoOfChildren.Text = assessmentCount == 1
                ? "1 Assessment"
                : $"{assessmentCount} Assessments";
        }

        if (InfoLabel1 != null && InfoLabel2 != null)
        {
            string lastName = GetLastName(course.InstructorName);
            int daysLeft = GetDaysUntil(DateTime.Now, course.EndDate);
            InfoLabel1.Text = $"Instr. {lastName}";
            InfoLabel2.Text = $"{daysLeft}ds left";
        }

        double progress = CalculateProgress(course.StartDate, course.EndDate);
        Progress = progress;
        if (ProgressBar != null)
        {
            ProgressBar.Progress = progress;
        }
    }

    private string GetSeason(DateTime date)
    {
        int month = date.Month;

        if (month >= 3 && month <= 5)
            return "Spring";

        if (month >= 6 && month <= 8)
            return "Summer";

        if (month >= 9 && month <= 11)
            return "Fall";

        return "Winter";
    }

    private int GetWeeksUntil(DateTime from, DateTime to)
    {
        if (to <= from)
            return 0;

        TimeSpan span = to - from;
        return (int)Math.Ceiling(span.TotalDays / 7.0);
    }

    private int GetDaysUntil(DateTime from, DateTime to)
    {
        if (to <= from)
            return 0;

        TimeSpan span = to - from;
        return (int)Math.Ceiling(span.TotalDays);
    }

    private string GetLastName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return string.Empty;
        string lastName = parts[parts.Length - 1];
        return lastName.Length > 12 ? lastName.Substring(0, 9) + "..." : lastName;
    }

    private double CalculateProgress(DateTime startDate, DateTime endDate)
    {
        DateTime now = DateTime.Now;

        if (now <= startDate)
            return 0.0;

        if (now >= endDate)
            return 100.0;

        TimeSpan totalDuration = endDate - startDate;
        if (totalDuration.TotalDays <= 0)
            return 100.0;

        TimeSpan elapsed = now - startDate;
        double progress = (elapsed.TotalDays / totalDuration.TotalDays) * 100.0;
        return Math.Clamp(progress, 0.0, 100.0);
    }
}
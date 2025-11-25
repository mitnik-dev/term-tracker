using TermTracker.Models;

namespace TermTracker.Views.Components;

public partial class CardCompact : ContentView
{
    public event EventHandler<object> CardClicked;

    public static readonly BindableProperty TermProperty =
        BindableProperty.Create(
            nameof(Term),
            typeof(Term),
            typeof(CardCompact),
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
            typeof(CardCompact),
            null,
            propertyChanged: OnModelChanged);

    public Course Course
    {
        get => (Course)GetValue(CourseProperty);
        set => SetValue(CourseProperty, value);
    }

    public CardCompact()
    {
        InitializeComponent();
    }

    private void OnFrameTapped(Object sender, TappedEventArgs e)
    {
        if (Term != null)
        {
            CardClicked?.Invoke(this, Term);
        }
        else if (Course != null)
        {
            CardClicked?.Invoke(this, Course);
        }
    }

    private static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardCompact control)
        {
            control.UpdateView();
        }
    }

    private void UpdateView()
    {
        Term term = Term;
        Course course = Course;

        if (term != null)
        {
            UpdateFromTerm(term);
        }
        else if (course != null)
        {
            UpdateFromCourse(course);
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
            DateRange.Text = $" {term.StartDate:MM/dd/yy} - {term.EndDate:MM/dd/yy}";
        }

        if (ButtonIndicator != null)
        {
            ButtonIndicator.Status = term.Status;
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
            DateRange.Text = $" {course.StartDate:MM/dd/yy} - {course.EndDate:MM/dd/yy}";
        }

        if (ButtonIndicator != null)
        {
            ButtonIndicator.Status = course.Status;
        }
    }
}


using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Components;

public partial class AssessmentCard : ContentView
{
    public static readonly BindableProperty AssessmentProperty =
        BindableProperty.Create(
            nameof(Assessment),
            typeof(Assessment),
            typeof(AssessmentCard),
            null,
            propertyChanged: OnAssessmentChanged);

    public Assessment Assessment
    {
        get => (Assessment)GetValue(AssessmentProperty);
        set => SetValue(AssessmentProperty, value);
    }

    public AssessmentCard()
    {
        InitializeComponent();
    }

    private static void OnAssessmentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AssessmentCard control)
        {
            control.UpdateView();
        }
    }

    private void UpdateView()
    {
        if (Assessment == null)
            return;

        // Update Type Label and Icon
        if (TypeLabel != null)
        {
            TypeLabel.Text = Assessment.Type.ToString();
        }

        if (TypeIcon != null)
        {
            string iconSource = Assessment.Type == AssessmentType.Objective
                ? "objective.png"
                : "performance.png";
            TypeIcon.Source = iconSource;
        }

        // Update Title
        if (TitleLabel != null)
        {
            TitleLabel.Text = Assessment.Name ?? $"{Assessment.Type} Assessment";
        }

        // Update Date Range
        if (DateRangeLabel != null)
        {
            DateRangeLabel.Text = $"{Assessment.StartDate:MM/dd/yy} - {Assessment.EndDate:MM/dd/yy}";
        }

        // Calculate and update progress
        if (ProgressBar != null)
        {
            double progress = CalculateProgress(Assessment.StartDate, Assessment.EndDate);
            ProgressBar.Progress = progress;
            ProgressBar.AcademicType = AcademicType.Assessments;
        }
    }

    private double CalculateProgress(DateTime startDate, DateTime endDate)
    {
        DateTime now = DateTime.Now;
        
        // If before start date, progress is 0
        if (now < startDate)
            return 0.0;
        
        // If after end date, progress is 100
        if (now > endDate)
            return 100.0;
        
        // Calculate progress based on elapsed time
        TimeSpan totalDuration = endDate - startDate;
        TimeSpan elapsedDuration = now - startDate;
        
        if (totalDuration.TotalDays <= 0)
            return 100.0;
        
        double progress = (elapsedDuration.TotalDays / totalDuration.TotalDays) * 100.0;
        return Math.Clamp(progress, 0.0, 100.0);
    }
}


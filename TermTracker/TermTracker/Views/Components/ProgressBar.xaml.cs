using TermTracker.Models.Enums;
namespace TermTracker.Views.Components;
public partial class ProgressBar : ContentView
{
    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(
            nameof(Progress),
            typeof(double),
            typeof(ProgressBar),
            0.0,
            propertyChanged: OnProgressChanged);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public static readonly BindableProperty AcademicTypeProperty =
        BindableProperty.Create(
            nameof(AcademicType),
            typeof(AcademicType),
            typeof(ProgressBar),
            AcademicType.Term,
            propertyChanged: OnAcademicTypeChanged);

    public AcademicType AcademicType
    {
        get => (AcademicType)GetValue(AcademicTypeProperty);
        set => SetValue(AcademicTypeProperty, value);
    }

    public ProgressBar()
    {
        InitializeComponent();
        UpdateProgress();
        UpdateTypeIndicators();
    }

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProgressBar control)
        {
            control.UpdateProgress();
        }
    }

    private static void OnAcademicTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProgressBar control)
        {
            control.UpdateTypeIndicators();
        }
    }

    private void UpdateProgress()
    {
        double normalized = Math.Clamp(Progress / 100.0, 0.0, 1.0);

        CompletionBar.Progress = normalized;
    }

    private void UpdateTypeIndicators()
    {
        switch (AcademicType)
        {
            case AcademicType.Term:
                Icon.Source = "term.png";
                break;
            case AcademicType.Courses:
                Icon.Source = "course.png";
                break;
            case AcademicType.Assessments:
                Icon.Source = "assessment.png";
                break;
        }
    }
}

using TermTracker.Models.Enums;

namespace TermTracker.Views.Components;

public partial class ProgressCircle : ContentView
{
    private class ProgressCircleDrawable : IDrawable
    {
        public double Progress { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (canvas == null || dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
                return;

            var size = 90f;
            var thickness = 8f;
            var effectiveSize = size - thickness;
            var x = thickness / 2;
            var y = thickness / 2;

            var progress = (int)Math.Clamp(Progress, 0, 100);

            Color secondaryColor = Colors.Gray;
            Color primaryColor = Colors.Blue;

            try
            {
                if (Application.Current?.Resources != null)
                {
                    if (Application.Current.Resources.TryGetValue("Secondary", out var secondary))
                        secondaryColor = secondary as Color ?? Colors.Gray;

                    if (Application.Current.Resources.TryGetValue("Primary", out var primary))
                        primaryColor = primary as Color ?? Colors.Blue;
                }
            }
            catch
            {
            }

            if (progress < 100)
            {
                float angle = GetAngle(progress);

                canvas.StrokeColor = secondaryColor;
                canvas.StrokeSize = thickness;
                canvas.DrawEllipse(x, y, effectiveSize, effectiveSize);

                canvas.StrokeColor = primaryColor;
                canvas.StrokeSize = thickness;
                canvas.DrawArc(x, y, effectiveSize, effectiveSize, 90, angle, true, false);
            }
            else
            {
                canvas.StrokeColor = primaryColor;
                canvas.StrokeSize = thickness;
                canvas.DrawEllipse(x, y, effectiveSize, effectiveSize);
            }
        }

        private float GetAngle(int progress)
        {
            float factor = 90f / 25f;

            if (progress > 75)
            {
                return -180 - ((progress - 75) * factor);
            }
            else if (progress > 50)
            {
                return -90 - ((progress - 50) * factor);
            }
            else if (progress > 25)
            {
                return 0 - ((progress - 25) * factor);
            }
            else
            {
                return 90 - (progress * factor);
            }
        }
    }

    public static readonly BindableProperty AcademicTypeProperty =
        BindableProperty.Create(
            nameof(AcademicType),
            typeof(AcademicType),
            typeof(ProgressCircle),
            AcademicType.Assessments,
            propertyChanged: OnAcademicTypeChanged);

    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(
            nameof(Progress),
            typeof(double),
            typeof(ProgressCircle),
            0.0,
            propertyChanged: OnProgressChanged);

    private ProgressCircleDrawable _drawable;

    public AcademicType AcademicType
    {
        get => (AcademicType)GetValue(AcademicTypeProperty);
        set => SetValue(AcademicTypeProperty, value);
    }

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public ProgressCircle()
    {
        InitializeComponent();
        _drawable = new ProgressCircleDrawable();
        ProgressGraphicsView.Drawable = _drawable;
        UpdateView();
    }

    private static void OnAcademicTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProgressCircle control)
        {
            control.UpdateView();
        }
    }

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProgressCircle control)
        {
            control.UpdateView();
        }
    }

    private void UpdateView()
    {

        TypeLabel.Text = AcademicType.ToString();

        if (IconImage == null || PercentageLabel == null || TypeLabel == null)
            return;

        string iconSource = AcademicType switch
        {
            AcademicType.Assessments => "assessment.png",
            AcademicType.Term => "term.png",
            AcademicType.Courses => "course.png",
        };

        IconImage.Source = iconSource;
        PercentageLabel.Text = $"{Progress:F0}%";
        TypeLabel.Text = AcademicType.ToString();

        if (_drawable != null)
        {
            _drawable.Progress = Math.Clamp(Progress, 0, 100);
            ProgressGraphicsView?.Invalidate();
        }
    }
}





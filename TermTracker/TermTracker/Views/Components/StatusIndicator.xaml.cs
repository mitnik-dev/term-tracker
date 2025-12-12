using TermTracker.Models.Enums;
namespace TermTracker.Views.Components;

public partial class StatusIndicator : ContentView
{
    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(
            nameof(Status),
            typeof(StatusType),
            typeof(StatusIndicator),
            StatusType.Future,
            propertyChanged: OnStatusChanged);

    public StatusType Status
    {
        get => (StatusType)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public StatusIndicator()
    {
        InitializeComponent();
        UpdateView();
    }

    private static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is StatusIndicator control)
        {
            control.UpdateView();
        }
    }
    private void UpdateView()
    {
        StatusLabel.Text = Status.ToString();

        switch (Status)
        {
            case StatusType.Active:
                IndicatorFrame.BackgroundColor = (Color)Application.Current.Resources["Warning"];
                break;
            case StatusType.Completed:
                IndicatorFrame.BackgroundColor = (Color)Application.Current.Resources["Success"];
                break;
            case StatusType.Future:
                IndicatorFrame.BackgroundColor = (Color)Application.Current.Resources["Info"];
                break;
            case StatusType.Dropped:
                IndicatorFrame.BackgroundColor = (Color)Application.Current.Resources["Danger"];
                break;
        }
    }
}
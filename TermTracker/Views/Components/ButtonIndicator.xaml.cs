using TermTracker.Models.Enums;
namespace TermTracker.Views.Components;

public partial class ButtonIndicator : ContentView
{
    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(
            nameof(Status),
            typeof(StatusType),
            typeof(ButtonIndicator),
            StatusType.Future,
            propertyChanged: OnStatusChanged);

    public StatusType Status
    {
        get => (StatusType)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public ButtonIndicator()
    {
        InitializeComponent();
        UpdateView();
    }

    private static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ButtonIndicator control)
        {
            control.UpdateView();
        }
    }
    private void UpdateView()
    {
        switch (Status)
        {
            case StatusType.Active:
                Icon.Source = "arrow_up_right.png";
                IconFrame.BackgroundColor = (Color)Application.Current.Resources["Primary"];
                break;
            case StatusType.Completed:
                Icon.Source = "check.png";
                IconFrame.BackgroundColor = (Color)Application.Current.Resources["Secondary"];
                break;
            case StatusType.Future:
                Icon.Source = "arrow_up_right.png";
                IconFrame.BackgroundColor = (Color)Application.Current.Resources["Primary"];
                break;
            case StatusType.Dropped:
                Icon.Source = "close.png";
                IconFrame.BackgroundColor = (Color)Application.Current.Resources["Secondary"];
                break;
        }
    }
}
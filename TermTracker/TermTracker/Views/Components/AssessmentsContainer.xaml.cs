using System.Collections.Specialized;
using TermTracker.Models;

namespace TermTracker.Views.Components;

public partial class AssessmentsContainer : ContentView
{
    public event EventHandler<Assessment> AssessmentClicked;
    public event EventHandler AddAssessmentClicked;

    public static readonly BindableProperty AssessmentsProperty =
        BindableProperty.Create(
            nameof(Assessments),
            typeof(IEnumerable<Assessment>),
            typeof(AssessmentsContainer),
            null,
            propertyChanged: OnAssessmentsChanged);

    public IEnumerable<Assessment> Assessments
    {
        get => (IEnumerable<Assessment>)GetValue(AssessmentsProperty);
        set => SetValue(AssessmentsProperty, value);
    }

    public static readonly BindableProperty ShowAddButtonProperty =
        BindableProperty.Create(
            nameof(ShowAddButton),
            typeof(bool),
            typeof(AssessmentsContainer),
            true,
            propertyChanged: OnShowAddButtonChanged);

    public bool ShowAddButton
    {
        get => (bool)GetValue(ShowAddButtonProperty);
        set => SetValue(ShowAddButtonProperty, value);
    }

    public AssessmentsContainer()
    {
        InitializeComponent();
        UpdateAssessments();
    }

    private static void OnAssessmentsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AssessmentsContainer control)
        {
            if (oldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= control.OnCollectionChanged;

            if (newValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += control.OnCollectionChanged;

            control.UpdateAssessments();
        }
    }

    private static void OnShowAddButtonChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AssessmentsContainer control)
        {
            control.UpdateAssessments();
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateAssessments();
    }

    public void ForceUpdate()
    {
        UpdateAssessments();
    }

    private void UpdateAssessments()
    {
        if (AssessmentsStack == null)
            return;

        AssessmentsStack.Children.Clear();

        if (Assessments != null)
        {
            foreach (var assessment in Assessments)
            {
                var assessmentCard = new AssessmentCard { Assessment = assessment };
                assessmentCard.AssessmentClicked += OnAssessmentCardClicked;
                AssessmentsStack.Children.Add(assessmentCard);
            }
        }

        // Add the + button if ShowAddButton is true
        if (ShowAddButton)
        {
            var addButton = new Frame
            {
                BackgroundColor = (Color)Application.Current.Resources["BgLight"],
                BorderColor = (Color)Application.Current.Resources["BorderMuted"],
                CornerRadius = 12,
                HeightRequest = 40,
                Padding = 0,
                Margin = new Thickness(4, 0),
                HasShadow = false
            };

            var addButtonGrid = new Grid
            {
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Padding = 10
            };

            var addButtonImage = new Image
            {
                Source = "plus.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            addButtonGrid.Children.Add(addButtonImage);
            addButton.Content = addButtonGrid;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnAddButtonTapped;
            addButton.GestureRecognizers.Add(tapGesture);

            AssessmentsStack.Children.Add(addButton);
        }
    }

    private void OnAssessmentCardClicked(object sender, Assessment assessment)
    {
        AssessmentClicked?.Invoke(this, assessment);
    }

    private void OnAddButtonTapped(object sender, TappedEventArgs e)
    {
        AddAssessmentClicked?.Invoke(this, EventArgs.Empty);
    }
}

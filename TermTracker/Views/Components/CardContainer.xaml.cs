using System.Collections;
using System.Collections.Specialized;

namespace TermTracker.Views.Components;

public partial class CardContainer : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(CardContainer),
            null,
            propertyChanged: OnItemsSourceChanged);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(CardContainer),
            null,
            propertyChanged: OnItemTemplateChanged);

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public event EventHandler AddButtonClicked;
    public event EventHandler<object> CardClicked;

    public CardContainer()
    {
        InitializeComponent();
        UpdateItemTemplate();
        UpdateItemsSource();
        SetupCollectionViewSelection();
    }

    private void SetupCollectionViewSelection()
    {
        if (ItemsCollectionView != null)
        {
            ItemsCollectionView.SelectionMode = SelectionMode.Single;
        }
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardContainer control)
        {
            control.Unsubscribe(oldValue);
            control.Subscribe(newValue);
            control.UpdateItemsSource();
        }
    }

    private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CardContainer control)
        {
            control.UpdateItemTemplate();
        }
    }

    private void Subscribe(object source)
    {
        if (source is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged += OnCollectionChanged;
        }
    }

    private void Unsubscribe(object source)
    {
        if (source is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged -= OnCollectionChanged;
        }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        SetCollectionHeight();
    }

    private void UpdateItemTemplate()
    {
        if (ItemsCollectionView != null)
        {
            ItemsCollectionView.ItemTemplate = ItemTemplate;
        }
    }

    private void UpdateItemsSource()
    {
        if (ItemsCollectionView != null)
        {
            ItemsCollectionView.ItemsSource = ItemsSource;
            SetupCollectionViewSelection();
            SetCollectionHeight();
        }
    }

    private void SetCollectionHeight()
    {
        if (ItemsCollectionView == null)
            return;

        bool hasItems = HasItems();
        ItemsCollectionView.HeightRequest = hasItems ? -1 : 0;
    }

    private bool HasItems()
    {
        if (ItemsSource == null)
            return false;

        foreach (var _ in ItemsSource)
            return true;

        return false;
    }

    private void OnAddButtonTapped(object sender, TappedEventArgs e)
    {
        AddButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection?.FirstOrDefault() ??
                          (sender is CollectionView cv ? cv.SelectedItem : null);

        if (selectedItem != null)
        {
            CardClicked?.Invoke(this, selectedItem);

            if (sender is CollectionView collectionView)
            {
                Dispatcher.Dispatch(() =>
                {
                    collectionView.SelectedItem = null;
                });
            }
        }
    }
}


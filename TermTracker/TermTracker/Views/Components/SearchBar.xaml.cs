using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace TermTracker.Views.Components;

public partial class SearchBar : ContentView
{
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(SearchBar),
            "Search...",
            propertyChanged: OnPlaceholderChanged);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SearchBar searchBar && newValue is string placeholder)
        {
            searchBar.SearchEntry.Placeholder = placeholder;
        }
    }

    public event EventHandler<TextChangedEventArgs>? SearchTextChanged;

    public SearchBar()
    {
        InitializeComponent();
        BindingContext = this;

        // Remove underline on Android
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchTextChanged?.Invoke(this, e);
    }

    public void ClearSearch()
    {
        SearchEntry.Text = string.Empty;
    }
}


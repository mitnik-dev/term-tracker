using TermTracker.Models;

namespace TermTracker.Views.Components;

public partial class NoteCard : ContentView
{
    public static readonly BindableProperty NoteProperty =
        BindableProperty.Create(
            nameof(Note),
            typeof(Note),
            typeof(NoteCard),
            null,
            propertyChanged: OnNoteChanged);

    public Note Note
    {
        get => (Note)GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(NoteCard),
            string.Empty,
            propertyChanged: OnTitleChanged);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public NoteCard()
    {
        InitializeComponent();
    }

    private static void OnNoteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NoteCard control && newValue is Note note)
        {
            control.UpdateNote();
        }
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NoteCard control && control.TitleLabel != null)
        {
            control.TitleLabel.Text = (string)newValue ?? string.Empty;
        }
    }

    private void UpdateNote()
    {
        if (Note == null)
            return;

        if (ContentLabel != null)
        {
            ContentLabel.Text = Note.Content ?? string.Empty;
        }

        if (TitleLabel != null)
        {
            string title = !string.IsNullOrEmpty(Note.Title)
                ? Note.Title
                : (!string.IsNullOrEmpty(Title)
                    ? Title
                    : "Note");

            TitleLabel.Text = title;
        }
    }
}


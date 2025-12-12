using CommunityToolkit.Maui.Views;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Popups;

public partial class AddNotePopup : Popup
{
    private readonly Note _newNote;

    public event EventHandler<Note> NoteSaved;

    public string Title
    {
        get => _newNote.Title;
        set => _newNote.Title = value;
    }

    public string Content
    {
        get => _newNote.Content;
        set => _newNote.Content = value;
    }

    public int TypeIndex
    {
        get => (int)_newNote.Type;
        set => _newNote.Type = (NoteType)value;
    }

    public AddNotePopup(int courseId)
    {
        InitializeComponent();

        _newNote = new Note
        {
            Title = string.Empty,
            Content = string.Empty,
            Type = NoteType.large,
            CourseId = courseId
        };

        TypePicker.ItemsSource = new List<string> { "large", "small" };
        TypePicker.SelectedIndex = 0;

        BindingContext = this;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_newNote.Title))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Note title is required.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_newNote.Content))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Note content is required.", "OK");
            return;
        }

        NoteSaved?.Invoke(this, _newNote);
        Close();
    }
}

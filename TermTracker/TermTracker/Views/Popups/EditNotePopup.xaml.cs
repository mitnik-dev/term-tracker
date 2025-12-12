using CommunityToolkit.Maui.Views;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Popups;

public partial class EditNotePopup : Popup
{
    private readonly Note _editableNote;

    public event EventHandler<Note> NoteSaved;
    public event EventHandler<Note> NoteDeleted;

    public string Title
    {
        get => _editableNote.Title;
        set => _editableNote.Title = value;
    }

    public string Content
    {
        get => _editableNote.Content;
        set => _editableNote.Content = value;
    }

    public int TypeIndex
    {
        get => (int)_editableNote.Type;
        set => _editableNote.Type = (NoteType)value;
    }

    public EditNotePopup(Note note)
    {
        InitializeComponent();

        _editableNote = new Note
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Type = note.Type,
            CourseId = note.CourseId
        };

        TypePicker.ItemsSource = new List<string> { "large", "small" };
        TypePicker.SelectedIndex = (int)note.Type;

        BindingContext = this;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_editableNote.Title))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Note title is required.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_editableNote.Content))
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Note content is required.", "OK");
            return;
        }

        NoteSaved?.Invoke(this, _editableNote);
        Close();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Note",
            "Are you sure you want to delete this note?",
            "Delete",
            "Cancel");

        if (confirm)
        {
            NoteDeleted?.Invoke(this, _editableNote);
            Close();
        }
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_editableNote.Title) || string.IsNullOrWhiteSpace(_editableNote.Content))
        {
            await Application.Current.MainPage.DisplayAlert("Cannot Share", "Note must have a title and content to share.", "OK");
            return;
        }

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = _editableNote.Title,
            Text = $"{_editableNote.Title}\n\n{_editableNote.Content}"
        });
    }
}

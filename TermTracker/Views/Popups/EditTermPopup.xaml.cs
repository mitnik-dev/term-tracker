using CommunityToolkit.Maui.Views;
using TermTracker.Models;

namespace TermTracker.Views.Popups;

public partial class EditTermPopup : Popup
{
    private readonly Term _editableTerm;

    public event EventHandler<Term> TermSaved;
    public event EventHandler<int> TermDeleted;

    public EditTermPopup(Term term)
    {
        InitializeComponent();

        _editableTerm = new Term
        {
            Id = term.Id,
            Name = term.Name,
            StartDate = term.StartDate,
            EndDate = term.EndDate
        };

        BindingContext = _editableTerm;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        TermSaved?.Invoke(this, _editableTerm);
        Close();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Term",
            "Are you sure you want to delete this term? This will remove all related courses, assessments, and notes.",
            "Delete",
            "Cancel");

        if (confirm)
        {
            TermDeleted?.Invoke(this, _editableTerm.Id);
            Close();
        }
    }
}

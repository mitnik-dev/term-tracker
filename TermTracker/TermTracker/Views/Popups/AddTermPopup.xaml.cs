using CommunityToolkit.Maui.Views;
using TermTracker.Models;

namespace TermTracker.Views.Popups;

public partial class AddTermPopup : Popup
{
    private readonly Term _newTerm;

    public event EventHandler<Term> TermSaved;

    public AddTermPopup()
    {
        InitializeComponent();

        _newTerm = new Term
        {
            Name = string.Empty,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(1)
        };

        BindingContext = _newTerm;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_newTerm.EndDate < _newTerm.StartDate)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "End date must be after start date.", "OK");
            return;
        }

        TermSaved?.Invoke(this, _newTerm);
        Close();
    }
}

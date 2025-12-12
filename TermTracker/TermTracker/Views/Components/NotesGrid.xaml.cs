using System.Collections.Specialized;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Views.Components;

public partial class NotesGrid : ContentView
{
    public event EventHandler<Note> NoteClicked;
    public event EventHandler AddNoteClicked;

    public static readonly BindableProperty NotesProperty =
        BindableProperty.Create(
            nameof(Notes),
            typeof(IEnumerable<Note>),
            typeof(NotesGrid),
            null,
            propertyChanged: OnNotesChanged);

    public IEnumerable<Note> Notes
    {
        get => (IEnumerable<Note>)GetValue(NotesProperty);
        set => SetValue(NotesProperty, value);
    }

    private int _calculatedRows;

    public NotesGrid()
    {
        InitializeComponent();
        UpdateNotes();
    }

    private static void OnNotesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NotesGrid control)
        {
            if (oldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= control.OnCollectionChanged;

            if (newValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += control.OnCollectionChanged;

            control.UpdateNotes();
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateNotes();
    }

    public void ForceUpdate()
    {
        UpdateNotes();
    }

    private void UpdateNotes()
    {
        if (NotesFlexGrid == null)
            return;

        if (Notes == null)
        {
            _calculatedRows = 0;
            UpdateGridStructure();
            UpdateGridContent();
            return;
        }

        CalculateNumberOfRows();
        UpdateGridStructure();
        UpdateGridContent();
    }

    private void CalculateNumberOfRows()
    {
        _calculatedRows = 0;

        foreach (var note in Notes)
        {
            switch (note.Type)
            {
                case NoteType.large:
                    _calculatedRows += 2;
                    break;
                case NoteType.small:
                    _calculatedRows++;
                    break;
            }
        }

        _calculatedRows = (int)Math.Ceiling((double)_calculatedRows / 2);
    }

    private void UpdateGridStructure()
    {
        NotesFlexGrid.RowDefinitions.Clear();
        NotesFlexGrid.ColumnDefinitions.Clear();
        NotesFlexGrid.Children.Clear();

        NotesFlexGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        NotesFlexGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        int rowsToAdd = _calculatedRows > 0 ? _calculatedRows : 1;
        for (int i = 0; i < rowsToAdd; i++)
        {
            NotesFlexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }
    }

    private void UpdateGridContent()
    {
        NotesFlexGrid.Children.Clear();

        int currentRow = 0;
        int emptyRow = -1;

        if (Notes != null)
        {
            foreach (var note in Notes)
            {
                var noteCard = new NoteCard { Note = note };
                noteCard.NoteClicked += OnNoteCardClicked;

                if (note.Type == NoteType.large)
                {
                    Grid.SetRow((BindableObject)noteCard, currentRow);
                    Grid.SetColumnSpan((BindableObject)noteCard, 2);
                    NotesFlexGrid.Children.Add(noteCard);

                    currentRow++;
                }
                else if (note.Type == NoteType.small)
                {
                    if (emptyRow == -1)
                    {
                        Grid.SetRow((BindableObject)noteCard, currentRow);
                        Grid.SetColumn((BindableObject)noteCard, 0);
                        NotesFlexGrid.Children.Add(noteCard);

                        emptyRow = currentRow;
                        currentRow++;
                    }
                    else
                    {
                        Grid.SetRow((BindableObject)noteCard, emptyRow);
                        Grid.SetColumn((BindableObject)noteCard, 1);
                        NotesFlexGrid.Children.Add(noteCard);

                        emptyRow = -1;
                    }
                }
            }
        }

        // Add button

        if (emptyRow == -1)
        {
            var iconButtonEnd = new ImageButton
            {
                Source = "plus.png",
                BackgroundColor = (Color)Application.Current.Resources["BgLight"],
                Aspect = Aspect.AspectFit,
                Padding = 10,
                Margin = new Thickness(8, 6, 8, 6),
                CornerRadius = 12,
                HeightRequest = 40,
                BorderWidth = 1,
                BorderColor = (Color)Application.Current.Resources["BorderMuted"]
            };
            iconButtonEnd.Clicked += OnAddButtonClicked;

            Grid.SetColumn(iconButtonEnd, 0);
            Grid.SetColumnSpan(iconButtonEnd, 2);
            NotesFlexGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(iconButtonEnd, currentRow + 1);

            NotesFlexGrid.Children.Add(iconButtonEnd);
        }
        else
        {
            var iconButtonTiled = new ImageButton
            {
                Source = "plus.png",
                BackgroundColor = (Color)Application.Current.Resources["BgLight"],
                Aspect = Aspect.AspectFit,
                Padding = 72,
                Margin = 4,
                CornerRadius = 12,
                BorderWidth = 1,
                BorderColor = (Color)Application.Current.Resources["BorderMuted"]
            };
            iconButtonTiled.Clicked += OnAddButtonClicked;

            if (emptyRow != _calculatedRows - 1)
            {
                var lastRowItems = NotesFlexGrid.Children
                    .Where(c => Grid.GetRow((BindableObject)c) == _calculatedRows - 1)
                    .ToList();

                var emptyRowItems = NotesFlexGrid.Children
                    .Where(c => Grid.GetRow((BindableObject)c) == emptyRow)
                    .ToList();

                foreach (var item in lastRowItems)
                {
                    Grid.SetRow((BindableObject)item, emptyRow);
                }

                foreach (var item in emptyRowItems)
                {
                    Grid.SetRow((BindableObject)item, _calculatedRows - 1);
                }
            }
            Grid.SetColumn(iconButtonTiled, 1);
            Grid.SetRow(iconButtonTiled, _calculatedRows - 1);
            NotesFlexGrid.Children.Add(iconButtonTiled);
        }
    }

    private void OnNoteCardClicked(object sender, Note note)
    {
        NoteClicked?.Invoke(this, note);
    }

    private void OnAddButtonClicked(object sender, EventArgs e)
    {
        AddNoteClicked?.Invoke(this, e);
    }
}



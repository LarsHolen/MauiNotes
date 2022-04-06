using OneLineNotebook.DataAccess;
using OneLineNotebook.Models;
using System.Diagnostics;

namespace MauiNotes;

public partial class MainPage : ContentPage
{
	// The repository that contain the sqlite connection and calls
	private static NoteRepository noteRepository;

	// Keeping track of which note is selected.  And an enpty one for when there are nothing selected.
	private NoteModel selected;
	private NoteModel emptyNote;


	public MainPage()
	{
		InitializeComponent();
		// create the empty note and set it as selected
		emptyNote = new();
		selected = emptyNote;
		// Creating the DB/table if its not there.  And give the name to the db
		noteRepository = new("notesDb"); // LocalApplicationData


		// Load notes
		LoadNotes();
		

	}

	/// <summary>
	/// Loading the notes from the DB and show them in the collectionView
	/// </summary>
    private async void LoadNotes()
    {
		// Trying to load notes
		try
        {
			collectionView.ItemsSource = await noteRepository.LoadNotesAsync();
		} catch (Exception ex)
        {
			// On an exeption, create a not that shows error message
			NoteModel debugNote = new NoteModel();
			debugNote.Note = "Unable to create DB.  Please restart app. Error msg: " + ex.Message;
			collectionView.ItemsSource = new List<NoteModel>() {debugNote};
        } 
		
	}

	/// <summary>
	/// Button event for the Add note button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void Add_Button_Clicked(object sender, EventArgs e)
    {
        await SaveNote();
    }

    private async Task SaveNote()
    {
        // if nothing, return
        if (string.IsNullOrEmpty(noteEntry.Text)) return;
        // if the text is the same as the text in the selected one.  Clear the textfield and set selected to emptyNote, return
        if (noteEntry.Text == selected.Note)
        {
            SetEmptyNote();
            return;
        }
		// Note is valid, save it
		NoteModel noteToSave = new NoteModel
		{
			Note = noteEntry.Text,
			Date = DateTime.Now.ToString()
		};

		await noteRepository.SaveNoteAsync(noteToSave);
        // Clear fields
        SetEmptyNote();
        // reload notes into the collectionView
		collectionView.ItemsSource = await noteRepository.LoadNotesAsync();
	
    }

    /// <summary>
    /// Clear textfield, selected and collectionViews selectedItem object
    /// </summary>
    private void SetEmptyNote()
    {
        noteEntry.Text = String.Empty;
        selected = emptyNote;
        collectionView.SelectedItem = emptyNote;
    }

	/// <summary>
	/// On selecting a note in the collectionView.  Set textfield to noteModel.Note and selected to CurrentSelection object as NoteModel
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		// Get the selected and update textfield
		selected = e.CurrentSelection[0] as NoteModel;
		noteEntry.Text = selected.Note;
	}

	/// <summary>
	/// Delete button event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void Delete_Button_Clicked(object sender, EventArgs e)
	{
		// if null exit func
		if (selected is null || selected == emptyNote) return;
		// Try to delete note
		await noteRepository.DeleteNote(selected);
		// Update collectionView
		collectionView.ItemsSource = await noteRepository.LoadNotesAsync();
		// Clear selected, textfield text and collectioViews selected
		SetEmptyNote();
	}

	/// <summary>
	/// When you press enter in the textfield
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void OnTextCompleted(object sender, EventArgs e)
    {
		await SaveNote();

	}

	/// <summary>
	/// When you click and focus on the textfield
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnTextFocus(object sender, EventArgs e)
    {
		collectionView.IsEnabled = false;
		SetEmptyNote();

	}
	private void OutOfFocus(object sender, EventArgs e)
    {
		collectionView.IsEnabled = false;
	}

}


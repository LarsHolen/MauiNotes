using OneLineNotebook.DataAccess;
using OneLineNotebook.Models;
using System.Diagnostics;

namespace MauiNotes;

public partial class MainPage : ContentPage
{

	List<NoteModel> notes = new();
	private static SqliteDatabaseAccess sqliteDatabaseAccess;
	private NoteModel selected;


	public MainPage()
	{
		InitializeComponent();
		sqliteDatabaseAccess = new("notesDb"); // LocalApplicationData
		LoadNotes();
		

	}

    private async void LoadNotes()
    {
		try
        {
			collectionView.ItemsSource = await sqliteDatabaseAccess.LoadNotesAsync();
		} catch
        {
			Debug.WriteLine("Catch");
			Thread.Sleep(500);
			//LoadNotes();
        } finally
        {
			Debug.WriteLine("Fnally");
		}
		
	}

    private async void Button_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(noteEntry.Text)) return;
		if(noteEntry.Text == selected.Note)
        {
			noteEntry.Text = "";
			selected = new NoteModel();
			collectionView.SelectedItem = selected;
			return;
        }

		await sqliteDatabaseAccess.SaveNoteAsync(new NoteModel
		{
			Note = noteEntry.Text,
			Date = DateTime.Now.ToString()
		});
		noteEntry.Text = string.Empty;
		collectionView.ItemsSource = await sqliteDatabaseAccess.LoadNotesAsync();
	}


	void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		//string previous = (e.PreviousSelection.FirstOrDefault() as NoteModel)?.Note;
		string current = (e.CurrentSelection.FirstOrDefault() as NoteModel)?.Note;
		selected = e.CurrentSelection.FirstOrDefault() as NoteModel;
		Debug.WriteLine(current);
		noteEntry.Text = current;
		Debug.WriteLine(selected.Id);
	}

	private async void Delete_Button_Clicked(object sender, EventArgs e)
	{

		if (selected is null) return;

		await sqliteDatabaseAccess.DeleteNote(selected);
		collectionView.ItemsSource = await sqliteDatabaseAccess.LoadNotesAsync();
	}
}


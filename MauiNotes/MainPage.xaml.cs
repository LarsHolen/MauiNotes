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
		sqliteDatabaseAccess = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "notesDb")); // LocalApplicationData
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
		if (!string.IsNullOrEmpty(noteEntry.Text))
		{
			await sqliteDatabaseAccess.SaveNoteAsync(new NoteModel
			{
				Note = noteEntry.Text,
				Date = DateTime.Now.ToString()
			});
			noteEntry.Text = string.Empty;
			collectionView.ItemsSource = await sqliteDatabaseAccess.LoadNotesAsync();
		}
	}


	void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		string previous = (e.PreviousSelection.FirstOrDefault() as NoteModel)?.Note;
		string current = (e.CurrentSelection.FirstOrDefault() as NoteModel)?.Note;
		selected = e.CurrentSelection.FirstOrDefault() as NoteModel;
		Debug.WriteLine(current);
		Debug.WriteLine(selected.Id);
	}

	private async void Delete_Button_Clicked(object sender, EventArgs e)
	{

		if (selected is not null)
		{
			await sqliteDatabaseAccess.DeleteNote(selected);
			collectionView.ItemsSource = await sqliteDatabaseAccess.LoadNotesAsync();
		}
	}
}


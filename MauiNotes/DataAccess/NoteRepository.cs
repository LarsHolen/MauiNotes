using OneLineNotebook.Models;
using SQLite;
using System.Diagnostics;

namespace OneLineNotebook.DataAccess
{
    public class NoteRepository
    {
        private readonly SQLiteAsyncConnection _connection;
        private string path;

        public NoteRepository(string dbPath)
        {
            // Make a path for the DB in the localAppdata folder
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + dbPath);
            _connection = new SQLiteAsyncConnection(path);
            try
            {
                _connection.CreateTableAsync<NoteModel>().Wait();
            } catch (Exception e)
            {
                Debug.WriteLine("Error1: " + e.InnerException.Message);
                Debug.WriteLine("...:" + e.Message);
                Debug.WriteLine("Error2: " + e.InnerException.Message);
                Process.GetCurrentProcess().Kill();
            }
        
        }

        /// <summary>
        /// Loads the 20 first Notes
        /// </summary>
        /// <returns></returns>
        public Task<List<NoteModel>> LoadNotesAsync()
        {
            return _connection.Table<NoteModel>().OrderByDescending(i => i.Date ).ToListAsync();
            //_connection.Table<NoteModel>().ToListAsync();
        }


        /// <summary>
        /// Save a new note to the DB
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Task<int> SaveNoteAsync(NoteModel note)
        {

            return _connection.InsertAsync(note);
        }



        /// <summary>
        /// Delete note .
        /// </summary>
        
        public Task<int> DeleteNote(NoteModel note)
        {
            return _connection.DeleteAsync(note);
        }


    }
}

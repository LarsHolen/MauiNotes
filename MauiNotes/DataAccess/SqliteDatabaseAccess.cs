using OneLineNotebook.Models;
using SQLite;

namespace OneLineNotebook.DataAccess
{
    public class SqliteDatabaseAccess
    {
        private readonly SQLiteAsyncConnection _connection;

        public SqliteDatabaseAccess(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<NoteModel>().Wait();
        }

        /// <summary>
        /// Loads the 20 first Notes
        /// </summary>
        /// <returns></returns>
        public Task<List<NoteModel>> LoadNotesAsync()
        {
            return _connection.Table<NoteModel>().ToListAsync();
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

        /// <summary>
        /// Drop db .
        /// </summary>

        public string PathDB()
        {
            return _connection.DatabasePath;
        }

    }
}

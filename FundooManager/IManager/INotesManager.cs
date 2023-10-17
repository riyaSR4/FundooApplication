using FundooModel.Notes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooManager.IManager
{
    public interface INotesManager
    {
        public Task<int> AddNotes(Note note);
        public Note ArchiveNotes(int noteId, string email);
        public bool DeleteNote(int noteId, string email);
        public Note EditNotes(Note note);
        public bool EmptyTrash(string email);
        public IEnumerable<Note> GetAllArchievedNotes(string emailId);
        public IEnumerable<Note> GetAllNotes(string emailId);
        public IEnumerable<Note> GetAllPinnedNotes(string emailId);
        public IEnumerable<Note> GetAllTrashNotes(string email);
        public Note PinNote(int noteId, string email);
    }
}

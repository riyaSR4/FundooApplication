using FundooManager.IManager;
using FundooModel.Notes;
using FundooRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooManager.Manager
{
    public class NotesManager : INotesManager
    {
        public readonly INotesRepository notesRepository;
        public NotesManager(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }
        public Task<int> AddNotes(Note note)
        {
            var result = this.notesRepository.AddNotes(note);
            return result;
        }
        public Note ArchiveNotes(int noteId, string email)
        {
            var result = this.notesRepository.ArchiveNotes(noteId, email);
            return result;
        }
        public bool DeleteNote(int noteId, string email)
        {
            var result = this.notesRepository.DeleteNote(noteId, email);
            return result;
        }
        public Note EditNotes(Note note)
        {
            var result = this.notesRepository.EditNotes(note);
            return result;
        }
        public bool EmptyTrash(string email)
        {
            var result = this.notesRepository.EmptyTrash(email);
            return result;
        }
        public IEnumerable<Note> GetAllArchievedNotes(string emailId)
        {
            var result = this.notesRepository.GetAllArchievedNotes(emailId);
            return result;
        }
        public IEnumerable<Note> GetAllNotes(string emailId)
        {
            var result = this.notesRepository.GetAllNotes(emailId);
            return result;
        }
        public IEnumerable<Note> GetAllPinnedNotes(string emailId)
        {
            var result = this.notesRepository.GetAllPinnedNotes(emailId);
            return result;
        }
        public IEnumerable<Note> GetAllTrashNotes(string email)
        {
            var result = this.notesRepository.GetAllTrashNotes(email);
            return result;
        }
        public Note PinNote(int noteId, string email)
        {
            var result = this.notesRepository.PinNote(noteId, email);
            return result;
        }
        public bool DeleteNotesForever(int noteId, string email)
        {
            var result = this.notesRepository.DeleteNotesForever(noteId, email);
            return result;
        }
        public bool RestoreNotes(int noteId, string email)
        {
            var result = this.notesRepository.RestoreNotes(noteId, email);
            return result;
        }
    }
}

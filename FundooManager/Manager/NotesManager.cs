using FundooManager.IManager;
using FundooModel.Entity;
using FundooModel.Notes;
using FundooRepository.IRepository;
using Microsoft.AspNetCore.Http;
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
        public Note ArchiveNotes(int noteId, int userId)
        {
            var result = this.notesRepository.ArchiveNotes(noteId, userId);
            return result;
        }
        public bool DeleteNote(int noteId, int userId)
        {
            var result = this.notesRepository.DeleteNote(noteId, userId);
            return result;
        }
        public Note EditNotes(Note note)
        {
            var result = this.notesRepository.EditNotes(note);
            return result;
        }
        public bool EmptyTrash(int userId)
        {
            var result = this.notesRepository.EmptyTrash(userId);
            return result;
        }
        public IEnumerable<Note> GetAllArchievedNotes(int userId)
        {
            var result = this.notesRepository.GetAllArchievedNotes(userId);
            return result;
        }
        public IEnumerable<Note> GetAllNotes(int userId)
        {
            var result = this.notesRepository.GetAllNotes(userId);
            return result;
        }
        public IEnumerable<Note> GetAllPinnedNotes(int userId)
        {
            var result = this.notesRepository.GetAllPinnedNotes(userId);
            return result;
        }
        public IEnumerable<Note> GetAllTrashNotes(int userId)
        {
            var result = this.notesRepository.GetAllTrashNotes(userId);
            return result;
        }
        public Note PinNote(int noteId, int userId)
        {
            var result = this.notesRepository.PinNote(noteId, userId);
            return result;
        }
        public bool DeleteNotesForever(int noteId, int userId)
        {
            var result = this.notesRepository.DeleteNotesForever(noteId, userId);
            return result;
        }
        public bool RestoreNotes(int noteId, int userId)
        {
            var result = this.notesRepository.RestoreNotes(noteId, userId);
            return result;
        }
        public string Image(IFormFile file, int noteId)
        {
            var result = this.notesRepository.Image(file, noteId);
            return result;
        }
        public Note GetNotebyId(int userId, int noteId)
        {
            var result = this.notesRepository.GetNotebyId(userId, noteId);
            return result;
        }
        public IEnumerable<Note> GetRemainder(int userid)
        {
            var result = this.notesRepository.GetRemainder(userid);
            return result;
        }
        public Task<int> CreateCopyNote(int userId, int noteId)
        {
            var result = this.notesRepository.CreateCopyNote(userId, noteId);
            return result;
        }
    }
}

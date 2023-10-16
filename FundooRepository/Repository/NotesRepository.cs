using FundooModel.Notes;
using FundooRepository.Context;
using FundooRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Repository
{
    public class NotesRepository : INotesRepository
    {
        public readonly UserDbContext context;
        public NotesRepository(UserDbContext context)
        {
            this.context = context;
        }
        public Task<int> AddNotes(Note note)
        {
            this.context.Notes.Add(note);
            var result = this.context.SaveChangesAsync();
            return result;
        }
        public Note EditNotes(Note note)
        {
            var data = this.context.Notes.Where(x => x.Id == note.Id && x.EmailId == note.EmailId).FirstOrDefault();
            if(data != null)
            {
                this.context.Notes.Update(note);
                this.context.SaveChangesAsync();
                return note;
            }
            return null;
        }
        public IEnumerable<Note> GetAllNotes(string email)
        {
            var result = this.context.Notes.Where(x => x.EmailId == email).AsEnumerable();
            if(result != null)
            {
                return result;
            }
            return null;
        }
        public bool DeleteNote(int noteId, string email)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.EmailId == email).FirstOrDefault();
            if(result != null)
            {
                result.IsTrash = true;
                this.context.Notes.Update(result);
                var deleteResult = this.context.SaveChanges();
                if(deleteResult == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

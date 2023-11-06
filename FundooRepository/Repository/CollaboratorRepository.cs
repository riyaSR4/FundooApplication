using FundooModel.Notes;
using FundooRepository.Context;
using FundooRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace FundooRepository.Repository
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        public readonly UserDbContext context;

        Nlog nlog = new Nlog();
        public CollaboratorRepository(UserDbContext context)
        {
            this.context = context;
        }

        public Task<int> AddCollaborator(Collaborator collaborator)
        {
            this.context.Collaborator.Add(collaborator);
            var result = this.context.SaveChangesAsync();
            nlog.LogInfo("Added Collaborator Sucessful");

            return result;
        }

        public bool DeleteCollab(int noteId, int userId)
        {
            var result = this.context.Collaborator.Where(x => x.NoteId == noteId && x.ReceiverUserId == userId).FirstOrDefault();
            if (result != null)
            {
                this.context.Collaborator.Remove(result);
                var deleteResult = this.context.SaveChanges();
                if (deleteResult == 1)
                {
                    nlog.LogInfo("Deleted Collaborator Sucessful");

                    return true;
                }
            }
            nlog.LogInfo("Error in Deleting Collaborator");

            return false;
        }
        public IEnumerable<Collaborator> GetAllCollabNotes(int userId)
        {
            var result = this.context.Collaborator.Where(x => x.SenderUserId == userId || x.ReceiverUserId == userId).AsEnumerable();
            if (result != null)
            {
                nlog.LogInfo("Retrieved Notes  Sucessful");

                return result;
            }
            nlog.LogInfo("Error in Retrieve Collab");

            return null;
        }
        public IEnumerable<NotesCollab> GetAllNotesColllab(int userId)
        {
            List<NotesCollab> collab = new List<NotesCollab>();
            var result = this.context.Notes.Join(this.context.Collaborator.Where(X => X.SenderUserId == userId),
                Note => Note.Id,
                Collaborator => Collaborator.NoteId,
                (Note, Collaborator) => new NotesCollab
                {
                    NoteId = Note.Id,
                    Title = Note.Title,
                    Description = Note.Description,
                    Image = Note.Image,
                    Colour = Note.Colour,
                    Reminder = Note.Reminder,
                    IsArchive = Note.IsArchive,
                    IsPin = Note.IsPin,
                    IsTrash = Note.IsTrash,
                    CreatedDate = Note.CreatedDate,
                    CollabId = Collaborator.CollabId,
                    SenderUserId = Collaborator.SenderUserId,
                    ReceiverUserId = Collaborator.ReceiverUserId
                });
            foreach (var data in result)
            {
                if (data.SenderUserId == userId || data.ReceiverUserId == userId)
                {
                    collab.Add(data);
                }
            }
            nlog.LogInfo("Displayed All Collab Notes Successfully");
            return result;
        }
    }
}


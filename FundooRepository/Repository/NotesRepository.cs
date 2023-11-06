using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FundooModel.Entity;
using FundooModel.Notes;
using FundooRepository.Context;
using FundooRepository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Repository
{
    public class NotesRepository : INotesRepository
    {
        public const bool IS_CHECK = true;
        public readonly UserDbContext context;
        private readonly IDistributedCache distributedCache;
        public NotesRepository(UserDbContext context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public Task<int> AddNotes(Note note)
        {
            this.context.Notes.Add(note);
            var result = this.context.SaveChangesAsync();
            return result;
        }
        public Note EditNotes(Note note)
        {
            var data = this.context.Notes.Where(x => x.Id == note.Id && x.Id == note.Id).FirstOrDefault();
            if(data != null)
            {
                data.Id = note.Id;
                data.Title = note.Title;
                data.Description = note.Description;
                data.Image = note.Image;
                data.Colour = note.Colour;
                data.Reminder = note.Reminder;
                data.IsArchive = note.IsArchive;
                data.IsPin = note.IsPin;
                data.IsTrash = note.IsTrash;
                data.CreatedDate = note.CreatedDate;
                data.ModifiedDate = note.ModifiedDate;
                this.context.Notes.Update(data);
                this.context.SaveChangesAsync();
                return note;
            }
            return null;
        }
        
        public bool DeleteNote(int noteId, int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.Id == userId).FirstOrDefault();
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
        public IEnumerable<Note> GetAllTrashNotes(int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == userId && x.IsTrash.Equals(IS_CHECK)).AsEnumerable();
            return result;
        }
        public bool EmptyTrash(int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == userId && x.IsTrash.Equals(IS_CHECK)).ToList();
            foreach(var data in result)
            {
                this.context.Notes.Remove(data);
            }
            var empty = this.context.SaveChanges();
            if(empty != 0)
            {
                return true;
            }
            return false;
        }
        public Note PinNote(int noteId,  int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.Id == userId).FirstOrDefault();
            if(result != null)
            {
                result.IsPin = true;
                this.context.Notes.Update(result);
                this.context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public IEnumerable<Note> GetAllPinnedNotes(int userId)
        {
            var result = this.context.Notes.Where(x => x.Id.Equals(userId) && x.IsPin.Equals(IS_CHECK)).AsEnumerable();
            return result;
        }
        public IEnumerable<Note> GetAllArchievedNotes(int userId)
        {
            var result = this.context.Notes.Where(x => x.Id.Equals(userId) && x.IsArchive.Equals(IS_CHECK)).AsEnumerable();
            return result;
        }
        public Note ArchiveNotes(int noteId, int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.Id == userId).FirstOrDefault();
            if(result != null)
            {
                result.IsArchive = true;
                this.context.Notes.Update(result);
                this.context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public bool DeleteNotesForever(int noteId, int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.Id == userId).FirstOrDefault();
            if(result != null)
            {
                this.context.Notes.Remove(result);
                var deleteResult = this.context.SaveChanges();
                if (deleteResult != 0)
                    return true;
            }
            return false;
        }
        public bool RestoreNotes(int noteId, int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == noteId && x.Id == userId).FirstOrDefault();
            if (result != null)
            {
                result.IsTrash = false;
                this.context.Notes.Update(result);
                var restoreResult = this.context.SaveChanges();
                if (restoreResult != 0)
                    return true;
            }
            return false;
        }
        public string Image(IFormFile file, int noteId)
        {
            try
            {
                if (file == null)
                {
                    return null;
                }
                var stream = file.OpenReadStream();
                var name = file.FileName;
                Account account = new Account(
                     "dzpesfhyv",
                     "383472912723393",
                     "A64zmnU-bBchnSNURq3tDo7zBXE");

                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name, stream)
                };
                ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
                if (uploadResult != null && uploadResult.Uri != null)
                {
                    var imageUrl = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                    var data = this.context.Notes.Where(t => t.Id == noteId).FirstOrDefault();
                    if (data != null)
                    {
                        data.Image = uploadResult.Uri.ToString();
                        var result = this.context.SaveChanges();
                        return data.Image;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void PutListToCache(int userId)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
            var Enlist = this.context.Notes.Where(o => o.Id == userId);
            var jsonString = JsonConvert.SerializeObject(Enlist);
            distributedCache.SetString("noteList", jsonString, options);
        }
        public List<Note> GetListFromCache(string key)
        {
            var CacheString = this.distributedCache.GetString(key);
            return JsonConvert.DeserializeObject<IEnumerable<Note>>(CacheString).ToList();
        }
        public IEnumerable<Note> GetAllNotes(int userId)
        {
            var result = this.context.Notes.Where(x => x.Id == userId && x.IsArchive == false && x.IsTrash == false).AsEnumerable();//.Take(5)
           // WriteToJsonFile(result.ToList());
            if (result != null)
            {
                this.PutListToCache(userId);
                return result;
            }
            var data = this.GetListFromCache("noteList");
            return null;
        }
        //public void WriteToJsonFile(List<Note> notes)
        //{
        //    string fileName = @"D:\BridgeLabs Training\FundooApplication\FundooRepository\Notes.json";
        //    string result = JsonConvert.SerializeObject(notes);
        //    File.WriteAllText(fileName, result);
        //}

        //public void ReadFromJson()
        //{
        //    string fileName = @"D:\BridgeLabs Training\FundooApplication\FundooRepository\Notes.json";
        //    string[] json = File.ReadAllLines(fileName);
        //    int count = json.Length;
        //}
        //public void GetAllNotesWithoutId()
        //{
        //    var result = this.context.Notes.AsEnumerable();
        //    Dictionary<int, List<Note>> dict = new Dictionary<int, List<Note>>();
        //    var group = result.GroupBy(x => x.Id);
        //    foreach (var item in group)
        //    {
        //        dict.Add(item.Key, item.ToList());
        //    }

        //    string res = JsonConvert.SerializeObject(dict);
        //    string FileName = @"D:\BridgeLabs Training\FundooApplication\FundooRepository\Notes.json";
        //    File.WriteAllText(FileName, res);
        //}
        //public IEnumerable<Note> GetAllNotes(int userId)
        //{
        //    var result = this.context.Notes.Where(x => x.Id == userId).AsEnumerable();
        //    if(result != null)
        //    {
        //        return result;
        //    }
        //    return null;
        //}

        public Note GetNotebyId(int userId, int noteId)
        {
            var data = this.GetListFromCache("noteList");
            if (data != null)
            {
                return data.Where(x => x.Id == userId && x.Id == noteId).FirstOrDefault();
            }
            else
            {
                var result = this.context.Notes.Where(x => x.Id == userId && x.Id == noteId).FirstOrDefault();
                return result;
            }
        }
        public IEnumerable<Note> GetRemainder(int userid)
        {
            var data = this.GetListFromCache("noteList");
            if (data != null)
            {
                return data.Where(x => x.Id == userid && x.Reminder != null).AsEnumerable();
            }
            else
            {
                var result = this.context.Notes.Where(x => x.Id == userid && x.Reminder != null).AsEnumerable();
                return result;
            }
        }
        public Task<int> CreateCopyNote(int userId, int noteId)
        {
            var data = this.context.Notes.Where(x => x.Id == userId && x.NoteId == noteId).FirstOrDefault();
            var note = new Note
            {
                Title = data.Title,
                Description = data.Description,
                Image = data.Image,
                Colour = data.Colour,
                Reminder = data.Reminder,
                IsArchive = data.IsArchive,
                IsPin = data.IsPin,
                IsTrash = data.IsTrash,
                CreatedDate = data.CreatedDate,
                ModifiedDate = data.ModifiedDate,
                Id = data.Id,
            };
            var result = this.AddNotes(note);
           return result;
        }

        //public Note AddNotesToFundoo(NotesEntity note, int userId)
        //{
        //    try
        //    {
        //        Note noteobj = new Note();
        //        var result = this.context.Notes.Where(x => x.Id == userId);
        //        if(result != null)
        //        {
        //            noteobj.Id = note.Id;
        //            noteobj.Title = note.Title;
        //            noteobj.Description = note.Description;
        //            noteobj.Image = note.Image;
        //            noteobj.Colour = note.Colour;
        //            noteobj.Reminder = note.Reminder;
        //            noteobj.IsArchive = note.IsArchive;
        //            noteobj.IsPin = note.IsPin;
        //            noteobj.IsTrash = note.IsTrash;
        //            noteobj.CreatedDate = note.CreatedDate;
        //            noteobj.ModifiedDate = note.ModifiedDate; 
        //            this.context.Notes.Add(noteobj);
        //            this.context.SaveChanges();
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //        return null;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}

using FundooManager.IManager;
using FundooModel.Entity;
using FundooModel.Notes;
using FundooModel.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        public readonly INotesManager notesManager;
        public NotesController(INotesManager notesManager)
        {
            this.notesManager = notesManager;
        }
        [HttpPost]
        [Route("AddNote")]
        public async Task<ActionResult> AddNote(Note note)
        {
            try
            {
                var result = await this.notesManager.AddNotes(note);
                if (result != 0)
                {
                    return this.Ok(new { Status = true, Message = "Note Added Successfully", Data = note });
                }
                return this.BadRequest(new { Status = false, Message = "Adding note Unsuccessful", Data = String.Empty });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { StatusCode = this.BadRequest(), Status = false, Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("ArchieveNote")]
        public ActionResult ArchiveNotes(int noteId, string email)
        {
            try
            {
                var result = this.notesManager.ArchiveNotes(noteId, email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Note Archieved Successfully", Data = result });
                }
                return this.BadRequest(new { Status = false, Message = "Archieving Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("DeleteNote")]
        public ActionResult DeleteNote(int noteId, string email)
        {
            try
            {
                var result = this.notesManager.DeleteNote(noteId, email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Note Deleted Successfully"});
                }
                return this.BadRequest(new { Status = false, Message = "Deleting Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("EditNote")]
        public ActionResult EditNote(Note note)
        {
            try
            {
                var result = this.notesManager.EditNotes(note);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Note Edited Successfully", Data = note });
                }
                return this.BadRequest(new { Status = false, Message = "Editing Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("EmptyTrash")]
        public ActionResult EmptyTrash(string email)
        {
            try
            {
                var result = this.notesManager.EmptyTrash(email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Trash Emptied" });
                }
                return this.BadRequest(new { Status = false, Message = "Empting Trash Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllNotes")]
        public async Task<ActionResult> GetAllNotes(string emailId)
        {
            try
            {
                var result = this.notesManager.GetAllNotes(emailId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "All Notes Found", data = result });
                }
                return this.BadRequest(new { Status = false, Message = "No Notes Found" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllArchievedNotes")]
        public async Task<ActionResult> GetAllArchievedNotes(string emailId)
        {
            try
            {
                var result = this.notesManager.GetAllArchievedNotes(emailId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "All Archieved Notes Found", data = result });
                }
                return this.BadRequest(new { Status = false, Message = "No Archieved Notes Found" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllPinnedNotes")]
        public async Task<ActionResult> GetAllPinnedNotes(string emailId)
        {
            try
            {
                var result = this.notesManager.GetAllPinnedNotes(emailId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "All Pinned Notes Found", data = result });
                }
                return this.BadRequest(new { Status = false, Message = "No Pinned Notes Found" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllTrashNotes")]
        public async Task<ActionResult> GetAllTrashNotes(string email)
        {
            try
            {
                var result = this.notesManager.GetAllTrashNotes(email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "All Trashed Notes Found", data = result });
                }
                return this.BadRequest(new { Status = false, Message = "No Trashed Notes Found" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("PinNote")]
        public ActionResult PinNote(int noteId, string email)
        {
            try
            {
                var result = this.notesManager.PinNote(noteId, email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Note Pinned Successfully", Data = result });
                }
                return this.BadRequest(new { Status = false, Message = "Pinning Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("DeleteNotesForever")]
        public ActionResult DeleteNotesForever(int noteId, string email)
        {
            try
            {
                var result = this.notesManager.DeleteNotesForever(noteId, email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Notes Deleted Forever" });
                }
                return this.BadRequest(new { Status = false, Message = "Notes Deletion Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("RestoreNotes")]
        public ActionResult RestoreNotes(int noteId, string email)
        {
            try
            {
                var result = this.notesManager.RestoreNotes(noteId, email);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Notes Restored Successfully", Data = result });
                }
                return this.BadRequest(new { Status = false, Message = "Restoring Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("CreateNote")]
        public ActionResult AddNotes(NotesEntity notesEntity)
        {
            try
            {

                string emailId = GetUserEmail(HttpContext);
                var result = this.notesManager.AddNotesToFundoo(notesEntity, emailId);
                if(result != null)
                {
                    return this.Ok(new { Status = true, Message = "Notes Created Successfully", Data = result });
                }
                return this.BadRequest(new { Status = false, Message = "Creating Note Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        public static string GetUserEmail(HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;
        }
    }
}

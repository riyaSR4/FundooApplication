using FundooManager.IManager;
using FundooModel.Notes;
using FundooModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                    return this.Ok(new { Status = true, Message = "Notes Added Successfully", Data = note });
                }
                return this.BadRequest(new { Status = false, Message = "Adding notes Unsuccessful", Data = String.Empty });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { StatusCode = this.BadRequest(), Status = false, Message = ex.Message });
            }
        }
    }
}

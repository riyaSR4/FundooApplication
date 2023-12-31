﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using FundooModel.User;

namespace FundooModel.Notes
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Colour { get; set; }
        public string Reminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int Id { get; set; }
        [ForeignKey("Id")]
        public Register register { get; set; }
        //[ForeignKey("Register")]
        //public int UserId { get; set; }

        //[JsonIgnore]
        //public virtual Register Register { get; set; }
    }
}

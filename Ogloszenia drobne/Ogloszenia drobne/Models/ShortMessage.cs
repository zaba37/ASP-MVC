using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class ShortMessage
    {
        [Key]
        public int ShortMessageId { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class BannedWord
    {
        [Key]
        public int BannedWordId { get; set; }
        public string Word { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class File
    {
       [Key] 
        public int FileId { get; set; }
        public int UserId { get; set; }
        
        public string Path { get; set; }

        public string Description { get; set; }

        public bool InDetails { get; set; } //czy plik bedzie wyswietlany czy jako zalacznik do pobrania

        public virtual RegisterViewModel User { get; set; }
    }
}
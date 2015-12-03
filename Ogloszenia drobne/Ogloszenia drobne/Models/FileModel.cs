using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
        
        public string Path { get; set; }

        public virtual RegisterViewModel User { get; set; }
    }
}
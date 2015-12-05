using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class Advertisement
    {
        [Key]
        public int AdvertisementId { get; set; }
        public string UserId { get; set; }

        public int CategoryId { get; set; }
       
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime AddedDate { get; set; }

        public int VisitCounter { get; set; }

        public bool Reported { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<File> Files { get; set; }

    }
}
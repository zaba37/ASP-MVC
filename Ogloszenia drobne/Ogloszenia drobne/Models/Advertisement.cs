﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [AllowHtml]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:dd-MM-yyyy}",ApplyFormatInEditMode=true)]
        public DateTime AddedDate { get; set; }

        public int VisitCounter { get; set; }

        public bool Reported { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<File> Files { get; set; }

       
        
        [NotMapped]

        public List<Category> CategoryList { get; set; }


    }
}
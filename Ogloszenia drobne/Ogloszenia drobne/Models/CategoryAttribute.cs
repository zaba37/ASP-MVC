using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class CategoryAttribute
    {
        [Key]
        [Column(Order = 1)] 
        public int CategoryId { get; set; }
        [Key]
        [Column(Order = 2)] 
        public int AttributeId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Attribute Attribute { get; set; }

        
    }
}
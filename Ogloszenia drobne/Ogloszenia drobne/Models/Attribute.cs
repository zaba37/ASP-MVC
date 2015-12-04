using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class Attribute
    {
       [Key]
        public int AttributeId { get; set; }
        public string Name { get; set; }

        public ICollection<CategoryAttribute> CategoriesAttributes { get; set; }

        public ICollection<AttributeValue> AttributiesValue { get; set; }
    }
}
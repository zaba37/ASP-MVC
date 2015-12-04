using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ogloszenia_drobne.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        public string Name { get; set; }

        public int Value { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace KBT.Models
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Image { get; set; }
        public DateTime DoB { get; set; }
        public List<Fruit> Fruits { get; set; }

    }
}

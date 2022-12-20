using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KBT.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public List<Fruit> Fruits { get; set; }

    }
}

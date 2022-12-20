using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace KBT.Models
{
    public class Fruit
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public int Price { get; set; }
        //foreign key
        [ForeignKey("Emlpoyees")]
        public int EmployeeId { get; set; }
        public virtual Employee Employees { get; set; }
        public List<Vendor> Vendors { get; set; }
    }
}

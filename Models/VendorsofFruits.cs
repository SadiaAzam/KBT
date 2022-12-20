using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KBT.Models
{
    public class VendorsofFruits
    {
        [Key]
        public int Id { get; set; }
        //foreign key
        [ForeignKey("Fruits")]
        public int FruitId { get; set; }
        public virtual Fruit Fruits { get; set; }
        //foreign key
        [ForeignKey("Vendors")]
        public int VendorId { get; set; }
        public virtual Vendor Vendors { get; set; }
    }
}

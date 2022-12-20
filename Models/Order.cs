using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KBT.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public OrderState Status { get; set; }
        public DateTime LastUpdated { get; set; }
        // Fruit
        [ForeignKey("Fruits")]
        public int FruitId { get; set; }
        public virtual Fruit Fruits { get; set; }
        // User
        [ForeignKey("Users")]
        public string UserId { get; set; }
        public virtual IdentityUser Users { get; set; }

    }
    public enum OrderState
    {
        InCart,
        OrderPlaced,
        Verifying,
        Inprocess,
        Delivered
    }
}

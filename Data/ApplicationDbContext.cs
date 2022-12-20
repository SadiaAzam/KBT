using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using KBT.Models;

namespace KBT.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KBT.Models.Fruit> Fruit { get; set; }
        public DbSet<KBT.Models.Vendor> Vendor { get; set; }
        public DbSet<KBT.Models.Employee> Employee { get; set; }
        public DbSet<KBT.Models.VendorsofFruits> VendorsofFruits { get; set; }
        public DbSet<KBT.Models.Order> Order { get; set; }
    }
}

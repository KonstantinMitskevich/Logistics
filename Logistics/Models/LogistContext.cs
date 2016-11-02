using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Logistics.Models
{
    public class LogistContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Tarif> Tarifs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

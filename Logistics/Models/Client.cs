using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Firm> Firms { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Client()
        {
            Orders = new List<Order>();
            Firms = new List<Firm>();
        }
    }
}
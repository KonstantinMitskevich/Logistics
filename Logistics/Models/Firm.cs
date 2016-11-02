using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Models
{
    public class Firm
    {
        public int Id { get; set; }

        [Required]
        [Display(Name= "Название компании")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Client> Clients { get; set; }
        public ICollection<Tarif> Tarifs { get; set; }

        public Firm()
        {
            Clients = new List<Client>();
            Tarifs = new List<Tarif>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Models
{
    public class Tarif
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Введите направление")]
        [Display(Name="Направление")]
        [MaxLength(50,ErrorMessage="Превышена максимальная длина записи")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Display(Name = "Цена")]
        [DataType(DataType.Currency)]
        public double Price { get; set; } //добавить валидацию цены

        public int FirmId { get; set; }
        public Firm Firm { get; set; } 
    }
}
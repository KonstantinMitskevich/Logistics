using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Logistics.Models
{
    public class Order
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage="Введите вес груза(кг)")]
        [Display(Name="Вес груза")]
        [RegularExpression("^[0-9]+$",ErrorMessage="Некорректный ввод")]
        public double CargoWeight { get; set; } 

        [Display(Name = "Клиент")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Тариф не определен")]
        [Display(Name = "Тариф")]
        [DataType(DataType.Currency)]
        public int TarifId { get; set; }
        public Tarif Tarif { get; set; }
    }
}
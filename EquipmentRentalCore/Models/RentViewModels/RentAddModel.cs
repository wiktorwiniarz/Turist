using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.RentViewModels
{
    public class RentAddModel
    {
        public RentAddModel()
        {
            RentStart = DateTime.Today.Date;
            RentEnd = DateTime.Today.AddMonths(3).Date;
            EquipmentList = new List<SelectListItem>();
        }
        [Display(Name = "Data ropoczęcia wypożyczenia")]
        [DataType(DataType.Date)]
        public DateTime RentStart { get; set; }
        [Display(Name = "Data zakończenia wypożyczenia")]
        [DataType(DataType.Date)]
        public DateTime RentEnd { get; set; }
        public int EquipmentID { get; set; }
        [Display(Name = "Jaki sprzęt wypożyczam")]
        public List<SelectListItem> EquipmentList { get; set; }
        public int UserID { get; set; }
        [Display(Name = "Który użytkownik wypożycza sprzęt")]
        public string Username { get; set; }
    }
}

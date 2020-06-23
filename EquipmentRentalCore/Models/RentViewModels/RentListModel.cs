using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.RentViewModels
{
    public class RentListModel
    {
        public int RentID { get; set; }
        public int EquipmentID { get; set; }
        [Display(Name = "Nazwa sprzętu")]
        public string EquipmentName { get; set; }
        [Display(Name = "Nazwa klienta co wypozycza sprzęt")]
        public string RentedByUser { get; set; }
        public int UserID { get; set; }
        [Display(Name = "Data wypożyczenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime RentStartDate { get; set; }
        [Display(Name = "Koniec wypożyczenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime RentEndDate { get; set; }

        public ProlongationRentalModel ProlongModalData { get; set; }
    }
}

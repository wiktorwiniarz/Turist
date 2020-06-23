using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.EquipmentViewModels
{
    public class ManageEquipmentModel
    {
        public ManageEquipmentModel()
        {

        }

        public ManageEquipmentModel(List<EquipmentType> typesList, List<Categor> categorsList)
        {
            EquipmentTypeList = new List<SelectListItem>();
            foreach (var item in typesList)
                EquipmentTypeList.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.TypeID.ToString()
                });

            CategorList = new List<SelectListItem>();
            foreach (var item in categorsList)
                CategorList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
        }
        public ManageEquipmentModel(List<EquipmentType> typesList, List<Categor> categorsList, int typeID, int categorID)
        {
            EquipmentTypeId = typeID;
            CategorID = categorID;
            EquipmentTypeList = new List<SelectListItem>();
            foreach (var item in typesList)
                EquipmentTypeList.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.TypeID.ToString(),
                    Selected = (item.TypeID == typeID) ? true : false
                });

            CategorList = new List<SelectListItem>();
            foreach (var item in categorsList)
                CategorList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item.Id == categorID) ? true : false
                });
        }
        [HiddenInput]
        public int? EquipmentId { get; set; }
        [Required(ErrorMessage = "Musisz podać nazwę swojego sprzętu", AllowEmptyStrings = false)]
        [Display(Name = "Nazwa sprzętu")]
        [StringLength(30, ErrorMessage = "Nazwa za długa")]
        public string EquipmentName { get; set; }
        [Required(ErrorMessage = "Typ sprzętu musi zostać wybrany")]
        public int EquipmentTypeId { get; set; }
        [Display(Name = "Typ sprzętu")]
        public List<SelectListItem> EquipmentTypeList { get; set; }
        [Required(ErrorMessage = "Kategoria musi zostać wybrana")]
        public int CategorID { get; set; }
        [Display(Name = "Dodaj sprzęt do kategorii")]
        public List<SelectListItem> CategorList { get; set; }
    }
}

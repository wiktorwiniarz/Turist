using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EquipmentRentalCore.Models.RentViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace EquipmentRentalCore.Controllers
{
    public class RentalController : Controller
    {
        private readonly EquipmentRentalContext _context;
        private readonly UserManager<User> _userManager;

        public RentalController(EquipmentRentalContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string data = null, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var rentals = new List<Rental>();
            if (data == null)
                rentals = await _context.Rentals
                    .Include(r => r.RentalUser)
                    .Include(e => e.RentalEquipment)
                    .AsNoTracking()
                    .ToListAsync();
            else
                rentals = await _context.Rentals
                    .Include(r => r.RentalUser)
                    .Include(e => e.RentalEquipment)
                    .AsNoTracking()
                    .Where(x => x.RentalUser.Name.Contains(data) || x.RentalUser.Surname.Contains(data))
                    .ToListAsync();

            var listToView = new List<RentListModel>();
            foreach (var item in rentals)
                listToView.Add(new RentListModel
                {
                    RentStartDate = item.RentalStart.Date,
                    RentEndDate = item.RentalEnd.Date,
                    RentedByUser = item.RentalUser.Name + " " + item.RentalUser.Surname,
                    RentID = item.RentalID,
                    UserID = item.RentalUserID,
                    EquipmentName = item.RentalEquipment.EquipmentName,
                    EquipmentID = item.RentalEquipmentID
                });
            return View(listToView);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!id.HasValue)
                return NotFound();

            var rental = await _context.Rentals
                .Include(r => r.RentalUser)
                .Include(e => e.RentalEquipment)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RentalID == id);

            if (rental == null)
                return NotFound();

            var model = new RentListModel
            {
                RentStartDate = rental.RentalStart.Date,
                RentEndDate = rental.RentalEnd.Date,
                RentedByUser = rental.RentalUser.Name + " " + rental.RentalUser.Surname,
                RentID = rental.RentalID,
                UserID = rental.RentalUserID,
                EquipmentName = rental.RentalEquipment.EquipmentName,
                EquipmentID = rental.RentalEquipmentID
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Rent(int? id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(User.Identity.Name));
            if (!id.HasValue)
            {
                var notRentedEquipmentList = await _context.Equipments.Where(x => !x.RentID.HasValue).ToListAsync();
                if (notRentedEquipmentList.Any())
                {
                    var rent = new RentAddModel()
                    {
                        UserID = user.Id,
                        Username = User.Identity.Name
                    };
                    foreach (var item in notRentedEquipmentList)
                        rent.EquipmentList.Add(new SelectListItem
                        {
                            Value = item.EquipmentID.ToString(),
                            Text = item.EquipmentName
                        });

                    return View(rent);
                }
                return RedirectToAction("NotAvaliable", "Error");
            }
            else
            {
                var equipment = await _context.Equipments.FirstOrDefaultAsync(x => x.EquipmentID == id);
                var rent = new RentAddModel
                {
                    UserID = user.Id,
                    Username = User.Identity.Name
                };
                rent.EquipmentList.Add(new SelectListItem
                {
                    Value = equipment.EquipmentID.ToString(),
                    Text = equipment.EquipmentName
                });
                return View(rent);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rent(RentAddModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var rentModel = new Rental
                {
                    RentalStart = model.RentStart,
                    RentalEnd = model.RentEnd,
                    RentalEquipmentID = model.EquipmentID,
                    RentalUserID = model.UserID
                };
                await _context.Rentals.AddAsync(rentModel);
                var result = await _context.SaveChangesAsync();

                var updateEquipData = await _context.Equipments.FirstOrDefaultAsync(x => x.EquipmentID == model.EquipmentID);
                updateEquipData.RentID = rentModel.RentalID;

                result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index", "Rental");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Service")]
        public async Task<IActionResult> _ProlongRentalModal(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var elementToProlong = await _context.Rentals.FirstOrDefaultAsync(x => x.RentalID == id);

            var rent = new RentListModel
            {
                ProlongModalData = new ProlongationRentalModel
                {
                    RentID = id,
                }
            };
            return PartialView(rent);
        }
        
        [HttpPost]
        [Authorize(Roles = "Service")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ProlongRentalModal(RentListModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var elementToEdit = await _context.Rentals.FirstOrDefaultAsync(x => x.RentalID == model.ProlongModalData.RentID);
            elementToEdit.RentalEnd = elementToEdit.RentalEnd.AddMonths(model.ProlongModalData.MonthProlongation);
            _context.Entry(elementToEdit).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Service")]
        public async Task<IActionResult> _ConfirmDeleteModal(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var element = await _context.Rentals.FirstOrDefaultAsync(x => x.RentalID == id);
            var rent = new RentListModel
            {
                RentID = element.RentalID
            };
            return PartialView(rent);
        }

        [HttpPost]
        [Authorize(Roles = "Service")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ConfirmDeleteModal(RentListModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var elementToDelete = await _context.Rentals.FirstOrDefaultAsync(x => x.RentalID == model.RentID);
            _context.Rentals.Remove(elementToDelete);
            var result = await _context.SaveChangesAsync();

            var equipmentToModify = await _context.Equipments.FirstOrDefaultAsync(x => x.RentID == model.RentID);
            equipmentToModify.RentID = null;
            _context.Entry(equipmentToModify).State = EntityState.Modified;
            result = await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models;
using EquipmentRentalCore.Models.EquipmentTypeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRentalCore.Controllers
{
    public class EquipmentTypeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly EquipmentRentalContext _context;

        public EquipmentTypeController(UserManager<User> userManager, EquipmentRentalContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null, string data = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var list = new List<EquipmentType>();

            if (data == null)  
                list = await _context.EquipmentTypes
                    .Include(e => e.Equipments)
                    .AsNoTracking()
                    .ToListAsync();
            else
                list = await _context.EquipmentTypes
                    .Include(e => e.Equipments)
                    .AsNoTracking()
                    .Where(x => x.TypeName.Contains(data))
                    .ToListAsync();


            var elements = new List<ListEquipmentTypeViewModel>();
            foreach (var item in list)
                elements.Add(new ListEquipmentTypeViewModel(item.TypeID, item.TypeName, item.Equipments.ToList()));

            return View(elements);
        }

        [Authorize(Roles = "Service")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var element = await _context.EquipmentTypes.FirstOrDefaultAsync(x => x.TypeID.Equals(id));
            if (element != null)
            {
                var item = new ManageEquipmentTypeModel
                {
                    Id = element.TypeID,
                    Name = element.TypeName
                };
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Service")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManageEquipmentTypeModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var elementToModify = await _context.EquipmentTypes.FirstOrDefaultAsync(x => x.TypeID.Equals(model.Id));
                elementToModify.TypeName = model.Name;
                _context.Entry(elementToModify).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "Errors during saving of elements - please check!");
            }
            return View(model);
        }

        [Authorize(Roles = "Service")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var elementToDelete = await _context.EquipmentTypes.FirstOrDefaultAsync(x => x.TypeID.Equals(id));

            if (elementToDelete != null)
            {
                _context.EquipmentTypes.Remove(elementToDelete);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return NotFound();
        }

        [Authorize(Roles = "Service")]
        [HttpGet]
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new ManageEquipmentTypeModel());
        }

        [Authorize(Roles = "Service")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManageEquipmentTypeModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var elementToAdd = new EquipmentType
                {
                    TypeName = model.Name
                };
                await _context.EquipmentTypes.AddAsync(elementToAdd);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (id.HasValue)
            {
                var equipmentType = await _context.EquipmentTypes
                        .Include(e => e.Equipments)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.TypeID == id);
                var model = new ListEquipmentTypeViewModel
                {
                    Id = equipmentType.TypeID,
                    Name = equipmentType.TypeName,
                    EquipmentsAttachedList = equipmentType.Equipments.ToList()
                };
                return View(model);
            }
            return NotFound();

        }
    }
}
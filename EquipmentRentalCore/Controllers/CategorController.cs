using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EquipmentRentalCore.Models.CategorViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EquipmentRentalCore.Controllers
{
    public class CategorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly EquipmentRentalContext _context;

        public CategorController(UserManager<User> userManager, EquipmentRentalContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null, string data = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var categorList = new List<Categor>();

            if (data == null)
                categorList = await _context.Categors
                    .Include(x => x.Equipments)
                    .ToListAsync();
            else
                categorList = await _context.Categors
                    .Include(x => x.Equipments)
                    .Where(x => x.Name.Contains(data))
                    .ToListAsync();

            var elements = new List<ListCategorsModel>();
            foreach (var item in categorList)
                elements.Add(new ListCategorsModel
                {
                    Id = item.Id,
                    CategorName = item.Name,
                    EquipmentAttachedList = item.Equipments.ToList()
                });

            return View(elements);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var element = await _context.Categors.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (element != null)
            {
                var item = new ManageCategorsModel
                {
                    Id = element.Id,
                    Name = element.Name
                };
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManageCategorsModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var elementToModify = await _context.Categors.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
                elementToModify.Name = model.Name;
                _context.Entry(elementToModify).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "Errors during saving of elements - please check!");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var elementToDelete = await _context.Categors.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (elementToDelete != null)
            {
                _context.Categors.Remove(elementToDelete);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new ManageCategorsModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManageCategorsModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var elementToAdd = new Categor
                {
                    Name = model.Name
                };
                await _context.Categors.AddAsync(elementToAdd);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, string returnUrl = null)
        {
            var element = await _context.Categors
                .Include(e => e.Equipments)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (element != null)
            {
                var item = new ListCategorsModel
                {
                    Id = element.Id,
                    CategorName = element.Name,
                    EquipmentAttachedList = element.Equipments.ToList()
                };
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
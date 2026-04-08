// Controllers/EmployeeController.cs

using ManagementSystem.Services;
using MangementSystem.Models;
using MangementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // ✅ Admin + HR dono dekh sakte hain
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        // ✅ Sirf Admin - Create Form
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        // ✅ Sirf Admin - Create Submit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Employee emp)
        {
            if (!ModelState.IsValid) return View(emp);

            await _employeeService.AddAsync(emp);
            TempData["Success"] = "Employee successfully add ho gaya!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Sirf Admin - Edit Form
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return BadRequest();
            var emp = await _employeeService.GetByIdAsync(id);
            return emp is null ? NotFound() : View(emp);
        }

        // ✅ Sirf Admin - Edit Submit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Employee emp)
        {
            if (id != emp.Id) return BadRequest();
            if (!ModelState.IsValid) return View(emp);

            try
            {
                await _employeeService.UpdateAsync(emp);
                TempData["Success"] = "Employee successfully update ho gaya!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeService.ExistsAsync(id)) return NotFound();
                throw;
            }
        }

        // ✅ Sirf Admin - Delete Confirm Page
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            var emp = await _employeeService.GetByIdAsync(id);
            return emp is null ? NotFound() : View(emp);
        }

        // ✅ Sirf Admin - Delete Confirm Submit
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _employeeService.ExistsAsync(id)) return NotFound();
            await _employeeService.DeleteAsync(id);
            TempData["Success"] = "Employee successfully delete ho gaya!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Admin + HR - Details Page
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return BadRequest();
            var emp = await _employeeService.GetByIdAsync(id);
            return emp is null ? NotFound() : View(emp);
        }
    }
}
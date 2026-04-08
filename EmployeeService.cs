// Services/EmployeeService.cs

using MangementSystem.Data;
using MangementSystem.Models;
using MangementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _db;

        public EmployeeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
            => await _db.Employees.AsNoTracking().ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _db.Employees.FindAsync(id);

        public async Task<bool> ExistsAsync(int id)
            => await _db.Employees.AnyAsync(e => e.Id == id);

        public async Task AddAsync(Employee emp)
        {
            await _db.Employees.AddAsync(emp);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee emp)
        {
            _db.Employees.Update(emp);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp is null) return;
            _db.Employees.Remove(emp);
            await _db.SaveChangesAsync();
        }
    }
}
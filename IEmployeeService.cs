using MangementSystem.Models;

namespace MangementSystem.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(Employee emp);
        Task UpdateAsync(Employee emp);
        Task DeleteAsync(int id);
    }
}

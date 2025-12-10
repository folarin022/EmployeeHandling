
using EmployeeHandling.Data;
using EmployeeHandling.Dto.EmployeeModel;

namespace EmployeeHandling.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<bool> AddEmployee(AddEmployeeDto request, CancellationToken cancellationToken);
        Task<Employee> GetEmployeeById(Guid Id,CancellationToken cancellationToken);
        Task<List<Employee>> GetAllEmployee(CancellationToken cancellationToken);
        Task<bool> UpdateEmployee(Employee dto, CancellationToken cancellationToken);
        Task<bool> DeleteEmployee(Guid Id, CancellationToken cancellationToken);
        Task SaveChangesAsync();
    }
}

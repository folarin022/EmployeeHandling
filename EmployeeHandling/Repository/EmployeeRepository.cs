using EmployeeHandling.Context;
using EmployeeHandling.Data;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeManagement.Dto.EmployeeModel;
using EmployeeManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHandling.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEmployee(AddEmployeeDto request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
            };

            await _dbContext.Employees.AddAsync(employee, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteEmployee(Guid Id, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FindAsync(new object[] { Id }, cancellationToken);

            if (employee == null)
                return false;

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<List<Employee>> GetAllEmployee(CancellationToken cancellationToken)
        {
           return await _dbContext.Employees.ToListAsync(cancellationToken);
        }

        public async Task<Employee> GetEmployeeById(Guid Id, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmployee(Employee dto, CancellationToken cancellationToken)
        {
            _dbContext.Employees.Update(dto);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}

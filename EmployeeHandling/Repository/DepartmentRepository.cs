using EmployeeHandling.Context;
using EmployeeHandling.Data;
using EmployeeHandling.Dto.DepartmentModel;
using EmployeeHandling.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddDepartment(AddDepartmentDto dto, CancellationToken cancellationToken)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };

            await _dbContext.Departments.AddAsync(department, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteDepartment(Guid id, CancellationToken cancellationToken)
        {
            var department = await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);

            if (department == null)
                return false;

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<List<Department>> GetDepartment(CancellationToken cancellationToken)
        {
            return await _dbContext.Departments.ToListAsync(cancellationToken);
        }

        public async Task<Department> GetDepartmentById(Guid Id, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments.FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);
        }

        public async Task<bool> UpdateDEpartment(Guid Id, AddDepartmentDto request, CancellationToken cancellationToken)
        {
            var department = await _dbContext.Departments.FindAsync(new object[] { Id }, cancellationToken);
            if (department == null)
                return false;

            department.Name = request.Name;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}

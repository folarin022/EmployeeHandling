using EmployeeHandling.Data;
using EmployeeHandling.Dto.DepartmentModel;

namespace EmployeeHandling.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<bool> AddDepartment (AddDepartmentDto dto,CancellationToken cancellationToken);
        Task<List<Department>> GetDepartment (CancellationToken cancellationToken);
        Task<Department> GetDepartmentById (Guid Id,CancellationToken cancellationToken);
        Task<bool> UpdateDEpartment (Guid Id,AddDepartmentDto request,CancellationToken cancellationToken);
        Task<bool> DeleteDepartment (Guid Id,CancellationToken cancellationToken);
    }
}

using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeHandling.Service.Interface
{
    public interface IEmployeeService
    {
        Task<BaseResponse<EmployeeResponseDto>> AddEmployee(AddEmployeeDto request, CancellationToken cancellationToken);
        Task<BaseResponse<List<EmployeeResponseDto>>> GetAllEmployee(CancellationToken cancellationToken);
        Task<Employee> GetEmployeeById(Guid Id, CancellationToken cancellationToken);
        Task<List<SelectListItem>> GetDepartmentsForDropdown();
        Task<BaseResponse<bool>> DeleteEmployee(Guid Id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateEmployee(Guid id, EditEmployeeDto request, CancellationToken cancellationToken);
    }
}

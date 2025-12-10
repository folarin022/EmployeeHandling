
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.DepartmentModel;
using EmployeeHandling.Dto.EmployeeModel;

namespace EmployeeHandling.Service.Interface
{
    public interface IEmployeeService
    {
        Task<BaseResponse<EmployeeResponseDto>> AddEmployee(AddEmployeeDto request, CancellationToken cancellationToken);
        Task<BaseResponse<List<EmployeeResponseDto>>> GetAllEmployee(CancellationToken cancellationToken);
        Task<BaseResponse<EmployeeResponseDto>> GetEmployeeById(Guid Id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteEmployee(Guid Id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateEmployee(Guid Id, AddEmployeeDto dto, CancellationToken cancellationToken);
    }
}

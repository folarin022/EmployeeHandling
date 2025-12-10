using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.DepartmentModel;

namespace EmployeeHandling.Service.Interface
{
    public interface IDepartmentService
    {
        Task<BaseResponse<Department>> AddDepartment (string name);
        Task<BaseResponse<List<DepartmentResponseDto>>> GetAllDepartment ();
        Task<BaseResponse<DepartmentResponseDto>> GetDepartmentById(Guid Id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateDepartment(Guid Id, EditDepartmentDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteDepartment(Guid Id, CancellationToken cancellationToken);
    }
}

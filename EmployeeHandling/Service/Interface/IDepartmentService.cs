using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeManagement.Dto;
using EmployeeManagement.Dto.DepartmentModel;
using EmployeeManagement.Dto.EmployeeModel;

namespace EmployeeManagement.Service.Interface
{
    public interface IDepartmentService
    {
        Task<BaseResponse<Department>> AddDepartment (string name);
        Task<BaseResponse<List<DepartmentResponseDto>>> GetAllDepartment ();
        //Task<BaseResponse<DepartmentResponseDto>> GetDepartmentById (Guid Id,CancellationToken cancellationToken);
        //Task<BaseResponse<bool>> UpdateDepartment (Guid Id,AddDepartmentDto request, CancellationToken cancellationToken);
        //Task<BaseResponse<bool>> DeleteDepartment (Guid Id,CancellationToken cancellationToken);
    }
}

using EmployeeHandling.Context;
using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.DepartmentModel;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Repository.Interface;
using EmployeeHandling.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            ILogger<DepartmentService> logger,  
            ApplicationDbContext dbContext
        )
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<Department>> AddDepartment(string name)
        {
            var response = new BaseResponse<Department>();

            try
            {
                var department = new Department
                {
                    Name = name,
                };

                _dbContext.Departments.Add(department);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Department added successfully with ID: {DepartmentId}", department.Id);

                response.IsSuccess = true;
                response.Data = department;
                response.Message = "Department added successfully";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error adding department: {DepartmentName}", dto.Name);
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error adding department: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteDepartment(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);

            try
            {
                var isDeleted = await _departmentRepository.DeleteDepartment(id, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete department with ID: {DepartmentId}", id);
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Failed to delete department";
                    return response;
                }

                _logger.LogInformation("Department deleted successfully with ID: {DepartmentId}", id);
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Department deleted successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with ID: {DepartmentId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error deleting department: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<List<DepartmentResponseDto>>> GetAllDepartment()
        {
            var response = new BaseResponse<List<DepartmentResponseDto>>();
            _logger.LogInformation("Fetching all departments");

            try
            {
                var departments = await _dbContext.Departments
                    .Include(d => d.Employees)
                    .ToListAsync();

                var departmentDtos = departments.Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees.Select(e => new EmployeeResponseDto
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        Address = e.Address,
                        PhoneNumber = e.PhoneNumber,
                        DepartmentName = d.Name
                    }).ToList()
                }).ToList();

                _logger.LogInformation("Fetched {Count} departments", departmentDtos.Count);

                response.IsSuccess = true;
                response.Data = departmentDtos;
                response.Message = "Departments retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments");
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error fetching departments: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<DepartmentResponseDto>> GetDepartmentById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DepartmentResponseDto>();
            _logger.LogInformation("Fetching department with ID: {DepartmentId}", id);

            try
            {
                var department = await _dbContext.Departments
                    .Include(d => d.Employees)
                    .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

                if (department == null)
                {
                    _logger.LogWarning("Department not found with ID: {DepartmentId}", id);
                    response.IsSuccess = false;
                    response.Message = "Department not found";
                    return response;
                }

                var dto = new DepartmentResponseDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Employees = department.Employees.Select(e => new EmployeeResponseDto
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        PhoneNumber = e.PhoneNumber,
                        Address = e.Address,
                        DepartmentName = department.Name
                    }).ToList()
                };

                response.IsSuccess = true;
                response.Data = dto;
                response.Message = "Department retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department with ID: {DepartmentId}", id);
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error retrieving department: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<bool>> UpdateDepartment(Guid id, EditDepartmentDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var department = await _departmentRepository.GetDepartmentById(id, cancellationToken);

            if (department == null)
            {
                response.IsSuccess = false;
                response.Message = "Department not found";
                return response;
            }

            department.Name = request.Name;

            await _dbContext.SaveChangesAsync();

            response.IsSuccess = true;
            response.Data = true;
            response.Message = "Department updated successfully";
            return response;
        }

    }
}


using EmployeeHandling.Context;
using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Repository.Interface;
using EmployeeHandling.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHandling.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ILogger<EmployeeService> logger,
            ApplicationDbContext dbContext
        )
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<EmployeeResponseDto>> AddEmployee(AddEmployeeDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<EmployeeResponseDto>();

            try
            {
                var department = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);

                if (department == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Department not found";
                    return response;
                }

                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    OtherName = request.OtherName,
                    Gender = request.Gender,
                    Email = request.Email,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    DepartmentId = request.DepartmentId
                };

                _dbContext.Employees.Add(employee);
                _logger.LogInformation("Adding employee: {FirstName} {LastName}", request.FirstName, request.LastName);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Employee added with ID: {Id}", employee.Id);


                response.IsSuccess = true;
                response.Message = "Employee added successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<BaseResponse<bool>> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

            try
            {
                var isDeleted = await _employeeRepository.DeleteEmployee(id, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete employee with ID: {EmployeeId}", id);
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Failed to delete employee";
                    return response;
                }

                _logger.LogInformation("Employee deleted successfully with ID: {EmployeeId}", id);
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Employee deleted successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error deleting employee: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<List<EmployeeResponseDto>>> GetAllEmployee(CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<EmployeeResponseDto>>();
            _logger.LogInformation("Fetching all employees");

            try
            {
                var employees = await _dbContext.Employees
                    .Include(e => e.Department)
                    .ToListAsync(cancellationToken);

                var dtoList = employees.Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    OtherName = e.OtherName,
                    Gender = e.Gender,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Address = e.Address,
                    DepartmentId = e.DepartmentId,
                    Department = e.Department.Name
                }).ToList();

                response.IsSuccess = true;
                response.Data = dtoList;
                response.Message = "Employee retrieved successfully";
                _logger.LogInformation("Fetched {Count} employees", dtoList.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all employees");
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error fetching employees: {ex.Message}";
            }

            return response;
        }

        public async Task<List<SelectListItem>> GetDepartmentsForDropdown()
        {
            return await _dbContext.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<BaseResponse<bool>> UpdateEmployee(Guid id, EditEmployeeDto request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (employee == null)
            {
                return new BaseResponse<bool> { IsSuccess = false, Message = "Employee not found" };
            }

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.OtherName = request.OtherName;
            employee.Gender = request.Gender;
            employee.Email = request.Email;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Address = request.Address;
            employee.DepartmentId = request.DepartmentId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse<bool> { IsSuccess = true };
        }


    }
}

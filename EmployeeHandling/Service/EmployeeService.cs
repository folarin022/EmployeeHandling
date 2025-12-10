using EmployeeHandling.Context;
using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Service.Interface;
using EmployeeManagement.Repository.Interface;
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
            _logger.LogInformation("Adding new employee: {FirstName} {LastName}", request.FirstName, request.LastName);

            try
            {
                var department = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Name == request.DepartmentName, cancellationToken);

                if (department == null)
                {
                    _logger.LogWarning("Department not found: {DepartmentName}", request.DepartmentName);
                    response.IsSuccess = false;
                    response.Message = "Department not found";
                    return response;
                }

                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    DepartmentId = department.Id,
                };

                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Employee added successfully with ID: {EmployeeId}", employee.Id);

                response.IsSuccess = true;
                response.Data = new EmployeeResponseDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Address = employee.Address,
                    PhoneNumber = employee.PhoneNumber,
                    DepartmentName = department.Name
                };
                response.Message = "Employee added successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee: {FirstName} {LastName}", request.FirstName, request.LastName);
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error adding employee: {ex.Message}";
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
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Address = e.Address,
                    //DepartmentName =  e.Department?.Name
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

        public async Task<BaseResponse<EmployeeResponseDto>> GetEmployeeById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<EmployeeResponseDto>();
            _logger.LogInformation("Fetching employee with ID: {EmployeeId}", id);

            try
            {
                var employee = await _employeeRepository.GetEmployeeById(id, cancellationToken);

                if (employee == null)
                {
                    _logger.LogWarning("Employee not found with ID: {EmployeeId}", id);
                    response.IsSuccess = false;
                    response.Message = "Employee not found";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = new EmployeeResponseDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    DepartmentName = employee.Department.Name
                };
                response.Message = "Employee retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee with ID: {EmployeeId}", id);
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error retrieving Employee: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateEmployee(Guid id, AddEmployeeDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

            try
            {
                var employee = await _dbContext.Employees
                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (employee == null)
                {
                    _logger.LogWarning("Employee not found with ID: {EmployeeId}", id);
                    response.IsSuccess = false;
                    response.Message = "Employee not found";
                    response.Data = false;
                    return response;
                }

                var department = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Name == request.DepartmentName, cancellationToken);

                if (department == null)
                {
                    _logger.LogWarning("Department not found: {DepartmentName}", request.DepartmentName);
                    response.IsSuccess = false;
                    response.Message = "Department not found";
                    response.Data = false;
                    return response;
                }

                employee.FirstName = request.FirstName;
                employee.LastName = request.LastName;
                employee.Email = request.Email;
                employee.Address = request.Address;
                employee.PhoneNumber = request.PhoneNumber;
                employee.DepartmentId = department.Id;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Employee updated successfully with ID: {EmployeeId}", id);
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Employee updated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error updating employee: {ex.InnerException?.Message ?? ex.Message}";
            }

            return response;
        }
    }
}

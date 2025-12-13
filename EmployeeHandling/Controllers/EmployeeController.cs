using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHandling.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> RearPage(CancellationToken cancellationToken)
        {

            var response = await _employeeService.GetAllEmployee(cancellationToken);

            if (!response.IsSuccess || response.Data == null)
            {
                return View(new List<EmployeeResponseDto>());
            }

            var employees = response.Data.Select(d => new EmployeeResponseDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                OtherName = d.OtherName,
                Gender = d.Gender,
                Email = d.Email,
                Address = d.Address,
                PhoneNumber = d.PhoneNumber,
                Department = d.Department
            }).ToList();


            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var dto = new AddEmployeeDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                OtherName = string.Empty,
                Gender = string.Empty,
                Email = string.Empty,
                PhoneNumber = string.Empty,
                Address = string.Empty,
                DepartmentId = Guid.Empty,
                Departments = await _employeeService.GetDepartmentsForDropdown()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                dto.Departments = await _employeeService.GetDepartmentsForDropdown();
                return View(dto);
            }
 
            await _employeeService.AddEmployee(dto, cancellationToken);
            TempData.Success("Employee added successfully!");
            return RedirectToAction("RearPage");
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(Guid id, EditEmployeeDto request, CancellationToken cancellationToken)
        {


            if (!ModelState.IsValid)
                return View(request);

            var result = await _employeeService.UpdateEmployee(request.Id, request, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }

            TempData["ToastMessage"] = $"success|Employee updated successfully!";
            return RedirectToAction("RearPage");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteEmployee(id, cancellationToken);
            TempData.Success("Employee deleted successfully!");
            return RedirectToAction("RearPage");
        }

    }
}

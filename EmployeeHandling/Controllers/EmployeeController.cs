using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

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
                return View(Enumerable.Empty<EmployeeHandling.Dto.EmployeeModel.EmployeeViewModel>());

            var employeesForView = response.Data.Select(e => new EmployeeHandling.Dto.EmployeeModel.EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.DepartmentName
            }).ToList();

            return View(employeesForView);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var dto = new AddEmployeeDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
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
        public async Task<IActionResult> EditEmployee(Guid id, AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _employeeService.UpdateEmployee(id, dto, cancellationToken);
            TempData.Success("Employee updated successfully!");
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

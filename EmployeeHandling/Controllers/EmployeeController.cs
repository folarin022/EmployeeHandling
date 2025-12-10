using EmployeeHandling.Data;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.EmployeeModel;
using EmployeeHandling.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHandling.Controllers
{
    public class EmployeeController(IEmployeeService _employeeService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> RearPage(CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetAllEmployee(cancellationToken);

            if (response == null || !response.IsSuccess || response.Data == null)
            {
                return View(Enumerable.Empty<EmployeeHandling.Data.EmployeeViewModel>());
            }

            var employeesForView = response.Data.Select(e => new EmployeeHandling.Data.EmployeeViewModel
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.DepartmentName
            }).ToList();

            return View(employeesForView);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee() => View(new Dto.EmployeeModel.AddEmployeeDto
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            Email = string.Empty,
            PhoneNumber = string.Empty,
            Address = string.Empty,
            DepartmentId = Guid.Empty,
            DepartmentName = string.Empty
        });

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Dto.EmployeeModel.AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await _employeeService.AddEmployee(dto, cancellationToken);
            return RedirectToAction("RearPage");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id, CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetEmployeeById(id, cancellationToken);
            if (!response.IsSuccess || response.Data == null)
                return NotFound();
            var dto = new Dto.EmployeeModel.AddEmployeeDto
            {
                FirstName = response.Data.FirstName,
                LastName = response.Data.LastName,
                Email = response.Data.Email,
                PhoneNumber = response.Data.PhoneNumber,
                Address = response.Data.Address,
                DepartmentId = response.Data.DepartmentId,
                DepartmentName = response.Data.DepartmentName
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Guid id, Dto.EmployeeModel.AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await _employeeService.UpdateEmployee(id, dto, cancellationToken);
            return RedirectToAction("RearPage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteEmployee(id, cancellationToken);
            return RedirectToAction("RearPage");
        }
    }
}

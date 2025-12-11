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

            if (response == null || !response.IsSuccess || response.Data == null)
            {
                return View(Enumerable.Empty<EmployeeViewModel>());
            }

            var employeesForView = response.Data.Select(e => new EmployeeViewModel
            {
                //Id = e.Id,
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
        public async Task<IActionResult> CreateEmployee(AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                dto.Departments = await _employeeService.GetDepartmentsForDropdown();
                return View(dto);
            }

            await _employeeService.AddEmployee(dto, cancellationToken);
            return RedirectToAction("RearPage");
        }


        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id, CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetEmployeeById(id, cancellationToken);
            if (!response.IsSuccess || response.Data == null)
                return NotFound();

            var dto = new AddEmployeeDto
            {
                //Id = response.Data.Id,
                FirstName = response.Data.FirstName,
                LastName = response.Data.LastName,
                DepartmentId = response.Data.DepartmentId,
                DepartmentName = response.Data.DepartmentName
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Guid id, AddEmployeeDto dto, CancellationToken cancellationToken)
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

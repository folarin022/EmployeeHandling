using EmployeeHandling.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHandling.Controllers
{
    public class EmployeeController(IEmployeeService employeeService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> RearPages(CancellationToken cancellationToken)
        {
            var response = await employeeService.GetAllEmployee(cancellationToken);
            if (!response.IsSuccess || response.Data == null)
            {
                return View(new List<Dto.EmployeeModel.EmployeeResponseDto>());
            }
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View(new Dto.EmployeeModel.AddEmployeeDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                PhoneNumber = string.Empty,
                Address = string.Empty,
                DepartmentId = Guid.Empty,
                DepartmentName = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Dto.EmployeeModel.AddEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await employeeService.AddEmployee(dto, cancellationToken);
            return RedirectToAction("RearPage");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id, CancellationToken cancellationToken)
        {
            var response = await employeeService.GetEmployeeById(id, cancellationToken);
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
            await employeeService.UpdateEmployee(id, dto, cancellationToken);
            return RedirectToAction("FrontPage");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            await employeeService.DeleteEmployee(id, cancellationToken);
            return RedirectToAction("RearPage");
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using EmployeeHandling.Dto.DepartmentModel;
using EmployeeHandling.Service.Interface;

namespace EmployeeHandling.Controllers
{
    public class DepartmentController(IDepartmentService _departmentService) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> FrontPage()
        {
            var response = await _departmentService.GetAllDepartment();

            if (!response.IsSuccess || response.Data == null)
            {
                return View(new List<DepartmentDto>());
            }

            var departments = response.Data.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddDepartmentDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _departmentService.AddDepartment(dto.Name);
            return RedirectToAction("FrontPage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id,CancellationToken cancellationToken)
        {
            var response = await _departmentService.GetDepartmentById(id,cancellationToken);

            if (!response.IsSuccess || response.Data == null)
                return NotFound();

            var dto = new EditDepartmentDto
            {
                Id = response.Data.Id,
                Name = response.Data.Name
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, EditDepartmentDto dto,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _departmentService.UpdateDepartment(Id,dto,cancellationToken);

            return RedirectToAction("FrontPage");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
        {
            await _departmentService.DeleteDepartment(id,cancellationToken);
            return RedirectToAction("FrontPage");
        }
    }
}

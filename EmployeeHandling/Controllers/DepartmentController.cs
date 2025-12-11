using Microsoft.AspNetCore.Mvc;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.DepartmentModel;
using EmployeeHandling.Service.Interface;

namespace EmployeeHandling.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }



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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _departmentService.AddDepartment(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction("FrontPage");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var response = await _departmentService.GetDepartmentById(id, cancellationToken);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartmentDto dto,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _departmentService.UpdateDepartment(dto.Id, dto,cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction("FrontPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var result = await _departmentService.DeleteDepartment(id,cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("FrontPage");
        }
    }
}

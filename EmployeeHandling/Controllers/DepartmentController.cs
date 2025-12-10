using EmployeeManagement.Service.Interface;
using EmployeeManagement.Dto.DepartmentModel;
using Microsoft.AspNetCore.Mvc;
using EmployeeHandling.Dto;
using EmployeeHandling.Dto.DepartmentModel;

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
        public async Task<IActionResult> Create(AddDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _departmentService.AddDepartment(dto.Name);
            return RedirectToAction("FrontPage"); // Redirect to FrontPage to see the list
        }
    }
}

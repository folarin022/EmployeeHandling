using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeHandling.Dto.EmployeeModel
{
    public class AddEmployeeDto
    {
        [Required]
        public  string FirstName { get; set; }
        [Required]
        public  string LastName { get; set; }
        [Required]
        public  string OtherName { get; set; }
        [Required]
        public  string Email { get; set; }
        [Required]
        public  string Gender { get; set; }
        [Required]
        public  string PhoneNumber { get; set; }
        [Required]
        public  string Address { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
        [Required]
        public string  DepartmentName { get; set; }
        public List<SelectListItem> Departments { get; set; } = new();
    }
}

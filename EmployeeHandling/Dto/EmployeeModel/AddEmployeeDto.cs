using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeHandling.Dto.EmployeeModel
{
    public class AddEmployeeDto
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; } 
        public  string OtherName { get; set; } 
        public  string Email { get; set; }
        public  string Gender { get; set; }
        public  string PhoneNumber { get; set; }
        public  string Address { get; set; }
        public Guid DepartmentId { get; set; }
        public string  DepartmentName { get; set; }
        public List<SelectListItem> Departments { get; set; } = new();
    }
}

using EmployeeHandling.Data;

namespace EmployeeManagement.Dto.EmployeeModel
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Department Department { get; set; }
    }
}

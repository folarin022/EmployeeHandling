using EmployeeHandling.Data;

namespace EmployeeManagement.Dto.EmployeeModel
{
    public class EmloyeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

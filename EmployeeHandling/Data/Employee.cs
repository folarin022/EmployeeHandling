namespace EmployeeHandling.Data
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

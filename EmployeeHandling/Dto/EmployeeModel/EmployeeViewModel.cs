namespace EmployeeHandling.Dto.EmployeeModel
{
    public class EmployeeViewModel
    {
        public Guid Id { set; get; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; }
    }
}

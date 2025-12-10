

using EmployeeHandling.Data;

namespace EmployeeManagement.Dto.DepartmentModel
{
    public class DepartmentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}

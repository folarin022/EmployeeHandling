

using EmployeeHandling.Data;

namespace EmployeeHandling.Dto.DepartmentModel
{
    public class DepartmentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}

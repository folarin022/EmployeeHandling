using EmployeeHandling.Dto.EmployeeModel;
using EmployeeManagement.Dto.EmployeeModel;

namespace EmployeeHandling.Dto
{
    public class DepartmentResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EmployeeResponseDto> Employees { get; set; } = new List<EmployeeResponseDto>();
    }
}

using SystemControlHuman.Models.Employees;

namespace SystemControlHuman.Models.Shifts;

public class ShiftWithEmployeeDto
{
    public ShiftDto Shift { get; set; }
    public EmployeeDto Employee { get; set; }
}
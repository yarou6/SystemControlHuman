using System.Text.Json.Serialization;

namespace SystemControlHuman.Models.Employees;

public class EmployeeWithRoleDto
{
    
    [JsonPropertyName("employeeDto")]
    public EmployeeDto Employee { get; set; }
    
    [JsonPropertyName("roleDto")]
    public RoleDto Role { get; set; }
}
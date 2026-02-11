using System;

namespace SystemControlHuman.Models.Employees;

public class CreateEmployeeDto
{
    public EmployeeDto Employee { get; set; }
    public object? Credential { get; set; }
}
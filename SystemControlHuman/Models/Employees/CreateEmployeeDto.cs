using System;

namespace SystemControlHuman.Models.Employees;

public class CreateEmployeeDto
{
    public EmployeeApiDto Employee { get; set; }
    public CredentialDto? Credential { get; set; }
}
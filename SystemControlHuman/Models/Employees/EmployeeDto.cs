using System;

namespace SystemControlHuman.Models.Employees;

public class EmployeeDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Position { get; set; }

    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }

    
    public string FullName => $"{LastName} {FirstName}";
    public string Status => IsActive ? "Активен" : "Уволен";
}
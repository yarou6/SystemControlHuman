using System;

namespace SystemControlHuman.Models.Employees;

public class EmployeeDto
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public DateTimeOffset HireDate { get; set; }
    
    public bool IsActive { get; set; }

    
    public string FullName => $"{LastName} {FirstName}";
    public string Status => IsActive ? "Активен" : "Уволен";
}
using System;

namespace SystemControlHuman.Models.Shifts;

public class ShiftDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }

    public string Description { get; set; }

    public string TimeRange => $"{StartDateTime:HH:mm} - {EndDateTime:HH:mm}";
}
namespace SystemControlHumanAPI.DTO;

public class ShiftDTO
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string? Description { get; set; }
}
namespace SystemControlHumanAPI.DTO;

public class CredentialDTO
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }
}
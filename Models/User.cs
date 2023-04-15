namespace SharpCardAPI.Models;
public class User{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public bool IsPersonnel { get; set; } = false;
}


public class UserDTO{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
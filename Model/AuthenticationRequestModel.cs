namespace imageuploadandmanagementsystem.Model
{
    public record LoginRequest(string Email, string Password);  
    public record RegisterRequest(string Email, string Password, string ConfirmPassword);
    public record UserRequest(string Email);
}
namespace imageuploadandmanagementsystem.Model
{
    public record LoginRequest(string Email, string Password,string IdToken, string Code, short providerId,string codeVerifier);  
    public record RegisterRequest(string Email, string Password, string ConfirmPassword);
    public record UserRequest(string Email);
}
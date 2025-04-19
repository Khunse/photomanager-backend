using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<AuthenticationResponse>> LoginAsync(LoginRequest request);
    }
}
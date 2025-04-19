using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Service.UserService
{
    public interface IUserService
    {
        ResponseModel<bool> RegisterNewUser(string userEmail, string Password);
        Task<ResponseModel<UserInfoModel>> GetCurrentUserInfo(string userEmail);
    }
}
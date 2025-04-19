
using imageuploadandmanagementsystem.Common;
using imageuploadandmanagementsystem.Data.Repository;
using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseModel<UserInfoModel>> GetCurrentUserInfo(string userEmail)
        {
            var resp = new ResponseModel<UserInfoModel>();

            try
            {
                var userInfo = await _userRepository.GetUserByEmail(userEmail);
                if (userInfo == null)
                {
                    resp.IsSuccess = false;
                    resp.Message = "User not found";
                    return resp;
                }

                resp.IsSuccess = true;
                resp.Resp = new UserInfoModel(userInfo.Email, userInfo.UserName);
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :::" + ex.Message);
                resp.IsSuccess = false;
                resp.Message = "Failed to get user info";
                return resp;
            }
        }

        public ResponseModel<bool> RegisterNewUser(string userEmail, string Password)
        {
            var resp = new ResponseModel<bool>();
            try
            {
                   
         if(_userRepository.CheckUserExist(userEmail).Result)
         {
            resp.Message = "User already exist";
             return resp;
         }
        var (hashPass, salt) = Encryption.Encrypt(Password);
        var result = _userRepository.CreateUser(userEmail, hashPass,salt).Result;

            resp.IsSuccess = result;
            resp.Message = result ? "User created successfully" : "Failed to create user";
            
        return resp;

            }
            catch(Exception ex)
            {

             Console.WriteLine("Error :::" + ex.Message);
            resp.Message = "Failed to create user";
             return resp;

            }
         
        }
    }
}
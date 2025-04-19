namespace imageuploadandmanagementsystem.Data.Repository;

public interface IUserRepository
{
    Task<bool> CreateUser(string userEmail, string Password, string SaltKey);
    Task<bool> CheckUserExist(string userEmail);
    Task<UserTable> GetUserByEmail(string userEmail);
}
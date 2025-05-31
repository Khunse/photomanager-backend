
using Microsoft.EntityFrameworkCore;

namespace imageuploadandmanagementsystem.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext appContext,ILogger<UserRepository> logger)
        {
            _context = appContext;
            _logger = logger;
        }

        public Task<bool> CheckUserExist(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email.Equals(userEmail));
            return Task.FromResult(user != null);
        }

        public Task<bool> CreateSocailUser(string userEmail, string userName,int providerId)
        {
            var newUser = new UserTable
            {
                UserId = Guid.NewGuid().ToString(),
                Email = userEmail,
                UserName = userName,
                Password = string.Empty,
                SaltKey = string.Empty,
                IsSocialLogin = true,
                ProviderId = providerId,
                Created_At = DateTime.UtcNow,
                Is_Active = true
            };

            _context.Users.Add(newUser);
            return Task.FromResult(_context.SaveChanges() > 0);
        }

        public async Task<bool> CreateUser(string userEmail, string Password, string SaltKey)
        {
            var newUser = new UserTable
            {
                UserId = Guid.NewGuid().ToString(),
                Email = userEmail,
                Password = Password,
                SaltKey = SaltKey,
                UserName = userEmail,
                Created_At = DateTime.UtcNow,
                Is_Active = true
            };

            await _context.Users.AddAsync(newUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserTable> GetUserByEmail(string userEmail)
        {
            var userInfo = new UserTable();
            try
            {
            _logger.LogInformation("Fetching user info for email: {Email}", userEmail);
             userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

             if(userInfo is null) _logger.LogInformation("User not found for email: {Email}", userEmail);
             
               else 
               {

                _logger.LogInformation("User found: {User}", userInfo);
             _logger.LogInformation("User info fetched: {User}", userInfo);
               }
            return userInfo;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :::" + ex?.Message);
                _logger?.LogError("Error occurred while fetching user info: {Message}", ex?.Message);
            }

            return userInfo;
        }
    }
}
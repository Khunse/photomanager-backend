
using Microsoft.EntityFrameworkCore;

namespace imageuploadandmanagementsystem.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext appContext)
        {
            _context = appContext;
        }

        public Task<bool> CheckUserExist(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email.Equals(userEmail));
            return Task.FromResult(user != null);
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
            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));
            return userInfo;
        }
    }
}
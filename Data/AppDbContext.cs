
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace imageuploadandmanagementsystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserTable> Users { get; set; }
        public DbSet<LoginInfoTable> Logins { get; set; }
        public DbSet<ImageTable> Images { get; set; }
    }
    
    [Table("Images")]
    public class ImageTable
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string ImageName { get; set; } = null!;
        public string ImageType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Created_At {get; set;}
    }

    [Table("Users")]
    public class UserTable
    {
        [Key]
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SaltKey { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public bool IsSocialLogin { get; set; }
        public int ProviderId { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At {get; set;}
        public bool Is_Active {get; set;}
    }

    [Table("LoginInfo")]
    public class LoginInfoTable
    {
        [Key]
        public string Jti { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expire_time {get; set;}
        public DateTime Created_At {get; set;}
        public DateTime? Updated_At {get; set;}
        public bool Is_Active {get; set;}
    }

}

using Microsoft.EntityFrameworkCore;

namespace imageuploadandmanagementsystem.Data.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckImageExistAsync(string userEmail, string imageName)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if(userInfo is null)
            {
                return false;
            }

            var isImageExist = await _context.Images.AnyAsync(x => x.UserId.Equals(userInfo.UserId) && x.ImageName.Equals(imageName));
            return isImageExist;
        }

        public async Task<bool> DeleteImagesAsync(string userEmail, List<string> imageName)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if(userInfo is null)
            {
                return false;
            }

             _context.Images.RemoveRange(_context.Images.Where(x => x.UserId.Equals(userInfo.UserId) && imageName.Contains(x.ImageName)));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ImageTable>> GetImagesListAsync(string userEmail)
        {
            var imageLists = new List<ImageTable>();
            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if(userInfo is null)
            {
                return imageLists;
            }

            imageLists = await _context.Images.Where(x => x.UserId.Equals(userInfo.UserId)).ToListAsync();
            return imageLists;
        }

        public async Task<int> ImageCountAsync(string userEmail, string imageName)
        {
               var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if(userInfo is null)
            {
                return 0;
            }

            return await _context.Images.CountAsync(x => x.UserId.Equals(userInfo.UserId) && x.ImageName.Equals(imageName));
        }

        public async Task<bool> SaveImageMetaDataAsync(string userEmail, string imageName)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail));

            if(userInfo is null)
            {
                return false;
            }

            await _context.Images.AddAsync(new ImageTable
                {
                    UserId = userInfo.UserId,
                    ImageName = imageName,
                    Description = string.Empty,
                    Created_At = DateTime.UtcNow
                });

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
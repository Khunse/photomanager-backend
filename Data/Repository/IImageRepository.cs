namespace imageuploadandmanagementsystem.Data.Repository
{
    public interface IImageRepository
    {
        Task<bool> SaveImageMetaDataAsync(string userId, string imageNames);
        Task<List<ImageTable>> GetImagesListAsync(string userId);
         Task<bool> DeleteImagesAsync(string userId, List<string> imageName);
         Task<bool> CheckImageExistAsync(string userId, string imageName);
         Task<int> ImageCountAsync(string userId,string imageName);
    }
}
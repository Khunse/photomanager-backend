using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Service.ImageService;

public interface IImageService
{
    Task<List<Image>> GenerateTempUploadLinkAsync(string userEmail, List<Image> fileNames);
    Task<List<Image>> GetImagesListAsync(string userEmail);
    Task<bool> DeleteImagesAsync(string userEmail, List<string> imageNames);
    Task<List<Image>> GenerateTempGetLinkAsync(string userEmail, List<Image> fileNames);
   
}

namespace   imageuploadandmanagementsystem.Model
{
    public record ImageRequestModel(string Description, string Tags, List<Image> imageUrls);
    public record Image(string name, string description, string tags, string imageUrl, string imgType);
}
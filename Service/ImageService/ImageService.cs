using Amazon.S3;
using Amazon.S3.Model;
using imageuploadandmanagementsystem.Data.Repository;
using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Service.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "";
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository,IConfiguration configuration)
        {
            var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? throw new ArgumentNullException("AWS_ACCESS_KEY_ID");
            var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? throw new ArgumentNullException("AWS_SECRET_ACCESS_KEY");
            var awsConfig = configuration.GetSection("AWS");
            var awsRegion = awsConfig["Region"] ?? throw new ArgumentNullException("AWS_REGION");
            var awsServiceUrl = awsConfig["ServiceURL"] ?? throw new ArgumentNullException("AWS_SERVICE_URL");
            var useLocalStack = bool.Parse(awsConfig["UseLocalStack"] ?? "false");
            _bucketName = awsConfig["BucketName"] ?? throw new ArgumentNullException("AWS_BUCKET_NAME");

            // var awss3config = new AmazonS3Config{
            //     ServiceURL = awsServiceUrl,
            //     RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsRegion),
            //     ForcePathStyle = useLocalStack,
            // };

            // if(!string.IsNullOrEmpty(awsServiceUrl)) awss3config.ServiceURL = awsServiceUrl;

            // _s3Client = new AmazonS3Client(awsAccessKey,awsSecretKey,awss3config);
            _s3Client = new AmazonS3Client();
            _imageRepository = imageRepository;
        }

        public async Task<List<Image>> GenerateTempUploadLinkAsync(string userEmail,List<Image> fileNames)
        {
            var presignUrls = new List<Image>();

            // var isBucketExist =
            await _s3Client.EnsureBucketExistsAsync(_bucketName);
            // if (!isBucketExist)
            // {
            //     var bucketRequest = new PutBucketRequest
            //     {
            //         BucketName = _bucketName,
            //         UseClientRegion = true
            //     };
            //     await _s3Client.PutBucketAsync(bucketRequest);
            // }
            
            foreach(var filename in fileNames)
            {
                var isImageExist = await _imageRepository.CheckImageExistAsync(userEmail,filename.name);
                string? imgName;
                if (isImageExist)
                {
                    var imgCount = await _imageRepository.ImageCountAsync(userEmail,filename.name);
                    imgName = $"{filename.name}_{imgCount}";
                }
                else
                {
                    imgName = filename.name;
                }
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{userEmail}/{imgName}",
                Expires = DateTime.UtcNow.AddMinutes(5),
                Verb = HttpVerb.PUT,
                ContentType = filename.imgType,
            };

            var tmpurl =  await _s3Client.GetPreSignedURLAsync(request);

            var imgUrl = new Image(
                filename.name,
                "",
                "",
                tmpurl,
                filename.imgType
            );
            presignUrls.Add(imgUrl);

              if( ! await _imageRepository.SaveImageMetaDataAsync(userEmail,imgName,filename.imgType))
            {
                Console.Error.WriteLine("ERROR :: Fail to save image metadata to Database!!!");
            }

            }
            
          

        return presignUrls;
        }

        private async Task<string> GenerateTempImageUrlAsync(string userEmail,string fileName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{userEmail}/{fileName}",
                Expires = DateTime.UtcNow.AddMinutes(5),
                Verb = HttpVerb.GET,
                // ContentType = "image/png"
            };

            return  await _s3Client.GetPreSignedURLAsync(request);
        }

        private async Task<bool> DeleteImageS3Async(string userEmail, string imageName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{userEmail}/{imageName}"
            };

            var response = await _s3Client.DeleteObjectAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public async Task<List<Image>> GetImagesListAsync(string userEmail)
        {
            var respImg = new List<Image>();

            var imagesFromDB = await _imageRepository.GetImagesListAsync(userEmail);

            if(imagesFromDB.Count > 0)
            {
                foreach(var img in imagesFromDB)
                {
                    var imgUrl = await GenerateTempImageUrlAsync(userEmail,img.ImageName);
                    respImg.Add(
                        new Image(
                            img.ImageName,
                            img.Description,
                            "",
                            imgUrl,
                            img.ImageType
                            )
                            );
                }
            }

            return respImg;
        }

        public async Task<bool> DeleteImagesAsync(string userEmail, List<string> imageNames)
        {
            var isDeletedFromDB = await  _imageRepository.DeleteImagesAsync(userEmail,imageNames);
            
            foreach(var imageName in imageNames)
            {
                if(! await DeleteImageS3Async(userEmail,imageName))
                {
                    Console.Error.WriteLine($"ERROR :: Fail to delete image {imageName} from S3!!!");
                }
            }

            return isDeletedFromDB;
        }

        public async Task<List<Image>> GenerateTempGetLinkAsync(string userEmail, List<Image> fileNames)
        {
            var presignUrls = new List<Image>();

            foreach(var filename in fileNames)
            {
                var imgUrl = new Image(
                    filename.name,
                    "",
                    "",
                  await  GenerateTempImageUrlAsync(userEmail,filename.name),
                    filename.imgType
                );
                presignUrls.Add(imgUrl);
            }

            return presignUrls;
        }
    }
}
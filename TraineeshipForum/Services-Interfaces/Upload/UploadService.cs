using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace TraineeshipForum.Services_Interfaces.Upload
{
    public class UploadService : IUpload
    {
        public IConfiguration Configuration;

        public UploadService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public CloudBlobContainer GetBlobContainer(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference("images");
        }
    }
}

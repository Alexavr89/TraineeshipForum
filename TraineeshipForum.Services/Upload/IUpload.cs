using Microsoft.Azure.Storage.Blob;

namespace TraineeshipForum.Services.Upload
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}

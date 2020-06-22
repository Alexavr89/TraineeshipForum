using Microsoft.Azure.Storage.Blob;

namespace TraineeshipForum.Services_Interfaces.Upload
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}

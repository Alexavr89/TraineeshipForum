using System.Threading.Tasks;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Posts
{
    public interface IPost
    {
        Post GetById(int id);
        Task Add(Post post);
        Task Delete(int id);
    }
}

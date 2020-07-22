using System.Collections.Generic;
using System.Threading.Tasks;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Categories
{
    public interface ICategory
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll();
        Task Add(Category category);
        Task Delete(int id);
    }
}

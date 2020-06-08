using System.Collections.Generic;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Categories
{
    public interface ICategory
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll(); //1.06
    }
}

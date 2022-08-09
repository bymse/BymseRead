using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Models;

namespace BymseBooks.DataLayer.Repository
{
    public interface ITagRepository
    {
        IList<TagModel> GetTags(int userId);
        bool Exist(string title, int userId);
        bool ExistAll(IList<int> ids);
        void Insert(Tag tag);
    }
}
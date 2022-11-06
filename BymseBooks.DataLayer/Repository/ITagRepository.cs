using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public interface ITagRepository
    {
        IReadOnlyList<Tag> GetTags();
        bool Exist(string title);
        bool ExistAll(IList<int> ids);
        void Insert(Tag tag);
    }
}
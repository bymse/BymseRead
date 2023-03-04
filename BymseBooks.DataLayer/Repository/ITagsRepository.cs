using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public interface ITagsRepository
    {
        IReadOnlyList<Tag> Search(string query);
    }
}
using BymseRead.DataLayer.Entity;

namespace BymseRead.DataLayer.Repository
{
    public interface ITagsRepository
    {
        Tag[] Search(string query);
        Tag[] FindTagsByName(IEnumerable<string> names);
        Tag[] CreateTags(IEnumerable<string> names);
    }
}
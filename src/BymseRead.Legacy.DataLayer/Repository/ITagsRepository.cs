using BymseRead.Legacy.DataLayer.Entity;

namespace BymseRead.Legacy.DataLayer.Repository
{
    public interface ITagsRepository
    {
        Tag[] Search(string query);
        Tag[] FindTagsByName(IEnumerable<string> names);
        Tag[] CreateTags(IEnumerable<string> names);
    }
}
using BymseRead.DataLayer.Repository;

namespace BymseRead.Core;

public class TagsService
{
    private readonly ITagsRepository tagsRepository;

    public TagsService(ITagsRepository tagsRepository)
    {
        this.tagsRepository = tagsRepository;
    }

    public string[] SearchTags(string query, IReadOnlyList<string> chosen, int max)
    {
        var tags = tagsRepository.Search(query).Select(e => e.Title).ToArray();
        return tags
            .Except(chosen)
            .Take(max)
            .ToArray();
    }
}
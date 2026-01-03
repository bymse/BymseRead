using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBookmarksRepository
{
    Task<int> Add(Bookmark bookmark);
}
using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBookmarksRepository
{
    Task Add(Bookmark bookmark);
}
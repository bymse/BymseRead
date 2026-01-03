using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBookProgressRepository
{
    Task<int> Upsert(BookProgress bookProgress);
}
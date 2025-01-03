using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBookProgressRepository
{
    Task Upsert(BookProgress bookProgress);
}
using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBooksRepository
{
    Task Add(Book book);
    Task Update(Book book);
}
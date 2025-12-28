namespace BymseRead.Core.Repositories;

public interface IFilesRepository
{
    Task Add(File file);
    Task Delete(File file);
}
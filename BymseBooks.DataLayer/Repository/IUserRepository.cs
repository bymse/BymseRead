using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public interface IUserRepository
    {
        bool UserExists(int userId);
        int CreateUser(DateTime now);
        User? FindUser(string userName);
    }
}
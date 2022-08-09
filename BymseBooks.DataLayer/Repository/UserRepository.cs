using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly BooksDbContext context;

        public UserRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public bool UserExists(int userId) => context.Users.Any(e => e.UserId == userId);

        public int CreateUser(DateTime now)
        {
            var user = new User
            {
                CreatedDate = now,
            };
            context.Users.Add(user);
            context.SaveChanges();
            return user.UserId;
        }

        public User? FindUser(string userName)
        {
            return context.Users.FirstOrDefault(e => e.Name == userName);
        }
    }
}
using System.Transactions;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Models;
using BymseBooks.DataLayer.Models.Extensions;

namespace BymseBooks.DataLayer.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly BooksDbContext context;

        public TagRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public IList<TagModel> GetTags(int userId)
        {
            return context.Tags
                .Where(e => e.CreatorUserId == userId)
                .ToModelsList();
        }

        public bool Exist(string title, int userId)
        {
            return context.Tags
                .Where(e => e.Title == title)
                .Any(e => e.CreatorUserId == userId);
        }

        public bool ExistAll(IList<int> ids)
        {
            using var transaction = new TransactionScope();
            var any = ids.Any(id => !context.Tags.Any(tag => tag.TagId == id));
            transaction.Complete();
            return !any;
        }

        public void Insert(Tag tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();
        }
    }
}
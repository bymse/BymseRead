using System.Transactions;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly BooksDbContext context;

        public TagRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public IReadOnlyList<Tag> GetTags()
        {
            return context.Tags.ToArray();
        }

        public bool Exist(string title)
        {
            return context.Tags.Any(e => e.Title == title);
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
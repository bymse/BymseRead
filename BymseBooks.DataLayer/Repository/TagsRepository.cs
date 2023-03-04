using System.Transactions;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public class TagsRepository : ITagsRepository
    {
        private readonly BooksDbContext context;

        public TagsRepository(BooksDbContext context)
        {
            this.context = context;
        }


        public IReadOnlyList<Tag> Search(string query)
        {
            return context.Tags
                .Where(t => t.Title.StartsWith(query))
                .ToArray();
        }
    }
}
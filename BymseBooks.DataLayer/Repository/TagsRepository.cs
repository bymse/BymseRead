using System.Transactions;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseBooks.DataLayer.Repository
{
    public class TagsRepository : ITagsRepository
    {
        private readonly BooksDbContext context;

        public TagsRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public Tag[] Search(string query)
        {
            return context.Tags
                .Where(t => t.Title.StartsWith(query))
                .ToArray();
        }

        public Tag[] FindTagsByName(IEnumerable<string> names)
        {
            return context.Tags
                .Where(e => names.Contains(e.Title))
                .ToArray();
        }

        public Tag[] CreateTags(IEnumerable<string> names)
        {
            var tags = names.Select(e => new Tag
            {
                Title = e
            }).ToArray();
            
            context.Tags.AddRange(tags);
            context.SaveChanges();

            return tags;
        }
    }
}
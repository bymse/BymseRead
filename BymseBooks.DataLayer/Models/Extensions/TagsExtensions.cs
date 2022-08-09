using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Models.Extensions
{
    public static class TagsExtensions
    {
        public static IList<TagModel> ToModelsList(this IQueryable<Tag> tags) =>
            tags.Select(e => new TagModel
            {
                Title = e.Title,
                ColorCode = e.ColorCode,
                TagId = e.TagId
            }).ToArray();
    }
}
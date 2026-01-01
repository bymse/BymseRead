using BymseRead.Core.Entities;

namespace BymseRead.Core.Common.Models;

public static class BookmarkInfoMapper
{
    public static BookmarkInfo? Map(Bookmark? bookmark)
    {
        if (bookmark == null)
        {
            return null;
        }

        return new BookmarkInfo { Page = bookmark.Page, CreatedAt = bookmark.CreatedAt, };
    }
}

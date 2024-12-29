namespace BymseRead.Legacy.Core.Models;

public class BookmarksListModel
{
    public BookmarkModel[] CustomBookmarks { get; init; }
    public BookmarkModel? LastPageBookmark { get; init; }
}
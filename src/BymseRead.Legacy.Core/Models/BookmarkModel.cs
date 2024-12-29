using BymseRead.Legacy.DataLayer.Entity;

namespace BymseRead.Legacy.Core.Models;

public class BookmarkModel
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public BookmarkType Type { get; init; }
    public DateTime Date { get; init; }
    public int Page { get; init; }
    public ColorCode ColorCode { get; init; } = ColorCode.White;
}
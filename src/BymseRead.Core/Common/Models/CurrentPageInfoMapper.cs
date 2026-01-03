using BymseRead.Core.Entities;

namespace BymseRead.Core.Common.Models;

public static class CurrentPageInfoMapper
{
    public static CurrentPageInfo? Map(BookProgress? progress)
    {
        if (progress == null)
        {
            return null;
        }

        return new CurrentPageInfo
        {
            Page = progress.CurrentPage,
            CreatedAt = progress.CurrentPageChangeAt ?? progress.StartedAt
        };
    }
}
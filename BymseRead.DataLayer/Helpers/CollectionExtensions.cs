namespace BymseRead.DataLayer.Helpers
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T>? collection) =>
            collection == null || collection.Count == 0;
        
        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T>? collection) =>
            collection == null || collection.Count == 0;
    }
}
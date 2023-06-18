namespace BymseRead.Ui.Models;

public interface ITopBarItem
{
    ItemType Type { get; }
}

public enum ItemType
{
    Link,
}

public class LinkTopBarItem : ITopBarItem
{
    public ItemType Type => ItemType.Link;
    public string Text { get; init; }
    public string Url { get; init; }
    public string Css { get; init; }
}
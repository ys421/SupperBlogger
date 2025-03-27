namespace BloggerBlazorServer.Models;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

using System.ComponentModel.DataAnnotations;

namespace BloggerLibrary;

public class Article
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public required string Title { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Author name must be between 1 and 50 characters.")]
    public required string Author { get; set; }

    [Required]
    public required string Content { get; set; }

    public DateTime PublishDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPublished { get; set; }
}
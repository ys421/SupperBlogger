using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloggerBlazorServer.Data;

namespace BloggerBlazorServer.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(20)]
        public string Body { get; set; } = string.Empty;

        // 작성자 ID 및 네비게이션 프로퍼티
        public string? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 글의 공개 여부: true이면 공개, false이면 비공개
        public bool IsPublished { get; set; } = false;
    }
}

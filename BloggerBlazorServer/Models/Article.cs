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

        // 원 작성자 – 변경되지 않음
        public string? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public ApplicationUser? Creator { get; set; }

        // 최종 수정자 – 수정 시 업데이트
        public string? LastModifiedBy { get; set; }
        [ForeignKey("LastModifiedBy")]
        public ApplicationUser? LastModifier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // StartDate와 EndDate 추가
        public DateTime StartDate { get; set; } = DateTime.UtcNow; // 기본값으로 현재 시간 설정
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(30); // 기본값으로 30일 후 설정

        public bool IsPublished { get; set; } = false;
    }
}
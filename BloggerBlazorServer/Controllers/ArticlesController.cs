using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggerBlazorServer.Data;
using BloggerBlazorServer.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BloggerBlazorServer.Controllers
{
    [ApiController]
    [Route("Article")]
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ArticlesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // POST: /Article/Edit
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromForm] ArticleEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == model.Id);
            if (article == null)
            {
                return NotFound();
            }

            // 원 작성자(CreatedBy)는 변경하지 않고, 수정 내용 반영
            article.Title = model.Title;
            article.Body = model.Body;
            article.IsPublished = model.IsPublished;
            // CreatedAt은 유지합니다.
            // 최종 수정자(LastModifiedBy)는 수정 시 현재 사용자로 업데이트
            article.LastModifiedBy = model.LastModifiedBy;

            await _db.SaveChangesAsync();
            return Redirect("/articles");
        }
    }

    public class ArticleEditModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be at least 5 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(20, ErrorMessage = "Body must be at least 20 characters.")]
        public string Body { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        // 원 작성자는 수정 시 변경하지 않음 (폼에는 포함하지 않아도 됩니다)
        public string? CreatedBy { get; set; }

        // 최종 수정자 (폼에서 현재 로그인 사용자 ID를 전달)
        public string? LastModifiedBy { get; set; }
    }
}

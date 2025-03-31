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

            article.Title = model.Title;
            article.Body = model.Body;
            article.StartDate = model.StartDate;
            article.EndDate = model.EndDate;
            article.IsPublished = model.IsPublished;
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

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsPublished { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
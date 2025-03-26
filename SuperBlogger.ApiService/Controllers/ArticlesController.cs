using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggerLibrary;

namespace SuperBlogger.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var now = DateTime.Now;
            return await _context.Articles
                .Where(a => a.IsPublished && a.StartDate <= now && a.EndDate >= now)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var now = DateTime.Now;
            var article = await _context.Articles.FindAsync(id);
            if (article == null || !article.IsPublished || article.StartDate > now || article.EndDate < now)
            {
                return NotFound();
            }
            return article;
        }
    }
}
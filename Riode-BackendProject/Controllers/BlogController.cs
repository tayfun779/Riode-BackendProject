using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;
using Riode_BackendProject.ViewModels;
using System.Text.RegularExpressions;

namespace Riode_BackendProject.Controllers;

public class BlogController : Controller
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search,int? topicId,int page=1)
    {

        if (page < 1)
            page = 1;

        var allBlogs = await _context.Blogs.Include(x => x.BlogTopics).ThenInclude(x => x.Topic).ToListAsync();

        var pageCount = (int)Math.Ceiling((decimal)allBlogs.Count / 3);

        ViewBag.PageCount = pageCount;
        if (page > pageCount)
            page = pageCount;

        ViewBag.CurrentPage = page;

        var query = _context.Blogs.Skip((page - 1) * 3).Take(3).Include(x=>x.BlogTopics).ThenInclude(x=>x.Topic).AsQueryable();

        if (!String.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Name.Contains(search));
        }
        if (topicId is not null)
        {
            query = query.Where(x => x.BlogTopics.Any(t=>t.TopicId==topicId));
        }
        var blogs = await query.ToListAsync();

        BlogViewModel vm = new()
        {
            Blogs = blogs,
            Topics=await _context.Topics.Include(x=>x.BlogTopics).Where(x=>x.BlogTopics.Count>0).ToListAsync(),
        };

        return View(vm);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var blog=await _context.Blogs.Include(x=>x.BlogTopics).ThenInclude(x=>x.Topic).FirstOrDefaultAsync(x=>x.Id==id);

        if (blog is null)
            return NotFound();


        BlogDetailViewModel vm = new()
        {
            Blog=blog,
            RelatedBlogs = await _context.Blogs.Include(x => x.BlogTopics).Where(x => x.BlogTopics.Any(x => x.TopicId ==blog.BlogTopics.FirstOrDefault().TopicId) && x.Id!=id).Take(3).ToListAsync(),
            Topics = await _context.Topics.Include(x => x.BlogTopics).Where(x => x.BlogTopics.Count > 0).ToListAsync(),


        };
        return View(vm);
    }
}


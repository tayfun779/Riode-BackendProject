using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Helpers.Extensions;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        var topics = await _context.Topics.ToListAsync();
        ViewBag.Topics = topics;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateViewModel vm)
    {
        var topics = await _context.Topics.ToListAsync();
        ViewBag.Topics = topics;

        if (!ModelState.IsValid)
            return View();

        if (vm.BlogTopicIds.Count == 0)
        {
            ModelState.AddModelError("BlogTopicIds", "Topic qeyd olunmalidir");
            return View(vm);
        }

        if (!vm.Image.CheckFileSize(3000))
        {
            ModelState.AddModelError("Image", "Sekil olcusu maximum 3 mb olmalidir");
            return View();
        }

        if (!vm.Image.CheckFileType("image/"))
        {
            ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
            return View();
        }


        Blog blog = new()
        {
            Name = vm.Name,
            Author = vm.Author,
            Description = vm.Description,
            CreatedTime = DateTime.Now,
            BlogTopics = new List<BlogTopic>()
        };

        string fileName = $"{Guid.NewGuid()}-{vm.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await vm.Image.CopyToAsync(stream);
        }

        blog.ImagePath = fileName;

        foreach (var topicId in vm.BlogTopicIds)
        {
            BlogTopic blogTopic = new()
            {
                Blog = blog,
                TopicId = topicId
            };

            blog.BlogTopics.Add(blogTopic);
        }

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }


    public async Task<IActionResult> Update(int id)
    {
        var blog = await _context.Blogs.Include(x => x.BlogTopics).FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();

        BlogUpdateViewModel vm = new()
        {
            Author = blog.Author,
            Description = blog.Description,
            Name = blog.Name,
            BlogTopicIds = blog.BlogTopics.Select(x => x.TopicId).ToList()
        };


        var topics = await _context.Topics.ToListAsync();
        ViewBag.Topics = topics;

        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, BlogUpdateViewModel vm)
    {
        var topics = await _context.Topics.ToListAsync();
        ViewBag.Topics = topics;

        if (!ModelState.IsValid)
            return View(vm);

        var existBlog = await _context.Blogs.Include(x => x.BlogTopics).FirstOrDefaultAsync(x => x.Id == id);

        if (existBlog is null)
            return NotFound();


        if (vm.BlogTopicIds.Count == 0)
        {
            ModelState.AddModelError("BlogTopicIds", "Topic qeyd olunmalidir");
            return View(vm);
        }

        if (vm.Image is not null)
        {
            if (!vm.Image.CheckFileSize(3000))
            {
                ModelState.AddModelError("Image", "Sekil olcusu maximum 3 mb olmalidir");
                return View();
            }

            if (!vm.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
                return View();
            }

            string fileName = $"{Guid.NewGuid()}-{vm.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await vm.Image.CopyToAsync(stream);
            }

            existBlog.ImagePath = fileName;
        }

        existBlog.BlogTopics = new List<BlogTopic>();


        foreach (var topicId in vm.BlogTopicIds)
        {
            BlogTopic blogTopic = new() { BlogId = id, TopicId = topicId };
            existBlog.BlogTopics.Add(blogTopic);

        }


        existBlog.Name = vm.Name;
        existBlog.Author = vm.Author;
        existBlog.Description = vm.Description;

        _context.Blogs.Update(existBlog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Delete(int id)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    
}

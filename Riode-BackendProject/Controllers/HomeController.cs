using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;
using Riode_BackendProject.ViewModels;

namespace Riode_BackendProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        HomeViewModel vm = new()
        {
            Sliders = await _context.Sliders.ToListAsync(),
            Services = await _context.Services.ToListAsync(),
            Categories = await _context.Categories.ToListAsync(),
            BestSellerProducts = await _context.Products.Include(x => x.ProductImages).OrderByDescending(x => x.Rating).Take(4).ToListAsync(),
            OurFeaturedProducts = await _context.Products.Include(x => x.Category).Include(x => x.ProductImages).Where(x => x.Category.Name == "Our Featured").Take(5).ToListAsync(),
            Blogs = await _context.Blogs.Take(3).ToListAsync(),
            SaleProducts = await _context.Products.OrderBy(x => x.Price).Include(x => x.ProductImages).Take(3).ToListAsync(),
            LatestProducts = await _context.Products.OrderBy(x => x.Id).Include(x => x.ProductImages).Take(3).ToListAsync(),
            PopularProducts = await _context.Products.Include(x => x.ProductImages).OrderByDescending(x => x.Rating).Take(3).ToListAsync(),
            BestOfWeekProducts=await _context.Products.Include(x=>x.ProductImages).OrderBy(x => x.Count).Take(3).ToListAsync()

        };
        return View(vm);
    }

    public async Task<IActionResult> Subscribe(Subscribe subscribe)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var isExist = await _context.Subscribes.AnyAsync(x => x.Email.ToLower() == subscribe.Email.ToLower());

        if (isExist)
            return RedirectToAction("Index");

        await _context.Subscribes.AddAsync(subscribe);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.ViewModels;

namespace Riode_BackendProject.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ShopViewModel vm = new()
        {
            Categories = await _context.Categories.Include(x=>x.Products).Where(x=>x.Products.Count>0).ToListAsync(),
            Products = await _context.Products.Include(x => x.Category).Include(x => x.ProductImages).ToListAsync(),
        };
        
        return View(vm);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var product=await _context.Products.Include(x=>x.Brand).Include(x=>x.Category).Include(x=>x.ProductImages).FirstOrDefaultAsync(x=>x.Id==id);

        if (product is null)
            return NotFound();

        ProductDetailViewModel vm = new()
        {
            Product = product,
            RelatedProducts =await _context.Products.Include(x=>x.ProductImages).Include(x=>x.Category).Where(x=>x.CategoryId==product.CategoryId || x.BrandId == product.BrandId).Where(x=>x.Id!=id).ToListAsync()
        };

        return View(vm);
    }
}
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

    public async Task<IActionResult> Index(string? search, int? categoryId, int? brandId, int page = 1)
    {
        if (page < 1)
            page = 1;

        var allProducts = await _context.Products.ToListAsync();

        var pageCount = (int)Math.Ceiling((decimal)allProducts.Count / 3);

        ViewBag.PageCount = pageCount;
        if (page > pageCount)
            page = pageCount;

        ViewBag.CurrentPage = page;

        var query = _context.Products.Skip((page - 1) * 3).Take(3).AsQueryable();

        if (!String.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Name.Contains(search));
        }
        if (categoryId is not null)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }
        if (brandId is not null)
        {
            query=query.Where(x=>x.BrandId == brandId);
        }


        var products = await query.Include(x => x.Category).Include(x => x.ProductImages).ToListAsync();
        ShopViewModel vm = new()
        {
            Brands = await _context.Brands.Include(x => x.Products).Where(x => x.Products.Count > 0).ToListAsync(),
            Categories = await _context.Categories.Include(x => x.Products).Where(x => x.Products.Count > 0).ToListAsync(),
            Products = products,
        };

        return View(vm);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        ProductDetailViewModel vm = new()
        {
            Product = product,
            RelatedProducts = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Where(x => x.CategoryId == product.CategoryId || x.BrandId == product.BrandId).Where(x => x.Id != id).ToListAsync()
        };

        return View(vm);
    }
}
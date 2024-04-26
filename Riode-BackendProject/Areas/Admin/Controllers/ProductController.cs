using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Helpers.Extensions;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(x => x.Category).Include(x => x.ProductImages).ToListAsync();

        return View(products);
    }


    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        var brands = await _context.Brands.ToListAsync();
        ViewBag.Brands = brands;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel vm)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        var brands = await _context.Brands.ToListAsync();
        ViewBag.Brands = brands;

        if (!ModelState.IsValid)
            return View(vm);




        foreach (var image in vm.ProductImages)
        {

            if (!image.CheckFileSize(3000))
            {
                ModelState.AddModelError("ProductImages", "Sekil olcusu maximum 3 mb olmalidir");
                return View();
            }

            if (!image.CheckFileType("image/"))
            {
                ModelState.AddModelError("ProductImages", "Mutleq shekil olmalidir!!!");
                return View();
            }

        }

        Product product = new()
        {
            Name = vm.Name,
            BrandId = (int)vm.BrandId,
            CategoryId = (int)vm.CategoryId,
            Count = vm.Count,
            Description = vm.Description,
            DiscountPercent = vm.DiscountPercent,
            Price = vm.Price,
            Manufacturer = vm.Manufacturer,
            Material = vm.Manufacturer,
            Rating = vm.Rating,
            SKU = vm.SKU,
            ProductImages = new List<ProductImage>()
        };


        foreach (var img in vm.ProductImages)
        {
            string fileName = $"{Guid.NewGuid()}-{img.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }

            ProductImage productImage = new() { Path = fileName, Product = product };

            product.ProductImages.Add(productImage);
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        ProductUpdateViewModel vm = new()
        {
            Name = product.Name,
            BrandId = product.BrandId,
            CategoryId = product.CategoryId,
            Description = product.Description,
            DiscountPercent = product.DiscountPercent,
            Manufacturer = product.Manufacturer,
            Material = product.Material,
            Price = product.Price,
            SKU = product.SKU,
            Rating = product.Rating,
            Count = product.Count,
        };



        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        var brands = await _context.Brands.ToListAsync();
        ViewBag.Brands = brands;

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductUpdateViewModel vm)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        var brands = await _context.Brands.ToListAsync();
        ViewBag.Brands = brands;


        if (!ModelState.IsValid)
            return View();

        var existedProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (existedProduct is null)
            return NotFound();


        if (vm.ProductImages.Count > 0)
        {

            foreach (var image in vm.ProductImages)
            {

                if (!image.CheckFileSize(3000))
                {
                    ModelState.AddModelError("ProductImages", "Sekil olcusu maximum 3 mb olmalidir");
                    return View();
                }

                if (!image.CheckFileType("image/"))
                {
                    ModelState.AddModelError("ProductImages", "Mutleq shekil olmalidir!!!");
                    return View();
                }

            }
        }


        existedProduct.Name = vm.Name;
        existedProduct.SKU = vm.SKU;
        existedProduct.BrandId = (int)vm.BrandId;
        existedProduct.CategoryId = (int)vm.CategoryId;
        existedProduct.Material = vm.Material;
        existedProduct.Manufacturer = vm.Manufacturer;
        existedProduct.Description = vm.Description;
        existedProduct.Rating = vm.Rating;
        existedProduct.DiscountPercent = vm.DiscountPercent;
        existedProduct.Price = vm.Price;
        existedProduct.Count = vm.Count;


        if (vm.ProductImages.Count > 0)
        {
            foreach (var img in existedProduct.ProductImages)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", img.Path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.ProductImages.Remove(img);
            }

            foreach (var img in vm.ProductImages)
            {
                string fileName = $"{Guid.NewGuid()}-{img.FileName}";
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                ProductImage productImage = new() { Path = fileName, Product = existedProduct };

                existedProduct.ProductImages.Add(productImage);
            }

        }


        _context.Products.Update(existedProduct);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        return View(product);
    }
    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();


        foreach (var img in product.ProductImages)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", img.Path);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.ProductImages.Remove(img);
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }


    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);


        if (product is null)
            return NotFound();

        return View(product);
    }
}

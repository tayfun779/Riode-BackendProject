using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class BrandController : Controller
{
    private readonly AppDbContext _context;

    public BrandController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var Brands = await _context.Brands.ToListAsync();
        return View(Brands);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BrandCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View();

        var isExist = await _context.Brands.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower());

        if (isExist)
        {
            ModelState.AddModelError("Name", "Bu Brand artiq movcuddur");
            return View(vm);
        }


        Brand Brand = new() { Name = vm.Name };

        await _context.Brands.AddAsync(Brand);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

        if (Brand is null)
            return NotFound();

        BrandUpdateViewModel vm = new()
        {
            Name = Brand.Name,
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id, BrandUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var existBrand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

        if (existBrand is null)
            return NotFound();

        var isExist = await _context.Brands.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower() && x.Id != id);

        if (isExist)
        {
            ModelState.AddModelError("Name", "Bu adda Brand movcuddur");
            return View(vm);
        }


        existBrand.Name = vm.Name;

        _context.Brands.Update(existBrand);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }



    public async Task<IActionResult> Delete(int id)
    {
        var Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

        if (Brand is null)
            return NotFound();

        _context.Brands.Remove(Brand);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

}

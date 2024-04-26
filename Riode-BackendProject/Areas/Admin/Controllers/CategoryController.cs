using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View();

        var isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower());

        if (isExist)
        {
            ModelState.AddModelError("Name", "Bu category artiq movcuddur");
            return View(vm);
        }


        Category category = new() { Name = vm.Name };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
            return NotFound();

        CategoryUpdateViewModel vm = new()
        {
            Name = category.Name,
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id,CategoryUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var existCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (existCategory is null)
            return NotFound();

        var isExist=await _context.Categories.AnyAsync(x=>x.Name.ToLower()==vm.Name.ToLower() && x.Id!=id);

        if (isExist)
        {
            ModelState.AddModelError("Name", "Bu adda category movcuddur");
            return View(vm);
        }


        existCategory.Name = vm.Name;

        _context.Categories.Update(existCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }



    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
            return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }


}

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class SettingController : Controller
{
    private readonly AppDbContext _context;

    public SettingController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var settings = await _context.Settings.ToListAsync();

        return View(settings);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Setting setting)
    {
        if (!ModelState.IsValid)
            return View(setting);


        var isExist = await _context.Settings.AnyAsync(x => x.Key == setting.Key);

        if(isExist)
        {
            ModelState.AddModelError("Key", "key artiq movcuddur");
            return View(setting);
        }

        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);

        if (setting is null)
            return NotFound();

        return View(setting);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,Setting setting)
    {
        if (!ModelState.IsValid)
            return View(setting);


        var existSetting=await _context.Settings.FirstOrDefaultAsync(x=>x.Id== id);

        if (existSetting is null)
            return NotFound();


        existSetting.Value = setting.Value;

        _context.Update(existSetting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);


        if (setting is null)
            return NotFound();

        _context.Settings.Remove(setting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}

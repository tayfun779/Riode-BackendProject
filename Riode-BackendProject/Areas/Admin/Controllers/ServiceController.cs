using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;
using System.Runtime.InteropServices;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]

public class ServiceController : Controller
{

    private readonly AppDbContext _context;

    public ServiceController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View();

        Service service = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            Icon = vm.Icon
        };


        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);

        if (service is null)
            return NotFound();

        ServiceUpdateViewModel vm = new()
        {
            Name = service.Name,
            Description = service.Description,
            Icon = service.Icon
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,ServiceUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existService = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);

        if (existService is null)
            return NotFound();

        existService.Name = vm.Name;
        existService.Description = vm.Description;
        existService.Icon=vm.Icon;

        _context.Services.Update(existService);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);

        if (service is null)
            return NotFound();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}

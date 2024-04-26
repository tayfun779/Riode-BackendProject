using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Helpers.Extensions;
using Riode_BackendProject.Models;
using System.Text.RegularExpressions;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class SliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.ToListAsync();
        return View(sliders);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(SliderCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.CheckFileSize(3000))
        {
            ModelState.AddModelError("ProductImages", "Sekil olcusu maximum 3 mb olmalidir");
            return View(vm);
        }

        if (!vm.Image.CheckFileType("image/"))
        {
            ModelState.AddModelError("ProductImages", "Mutleq shekil olmalidir!!!");
            return View(vm);
        }

        Slider slider = new()
        {
            Title = vm.Title,
            Subtitle = vm.Subtitle,
            Description = vm.Description,
        };


        string fileName = $"{Guid.NewGuid()}-{vm.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await vm.Image.CopyToAsync(stream);
        }

        slider.ImagePath = fileName;


        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    
    public async Task<IActionResult> Update(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

        if (slider is null)
            return NotFound();

        SliderUpdateViewModel vm = new()
        {
            Title = slider.Title,
            Subtitle = slider.Subtitle,
            Description = slider.Description,
        };

        return View(vm);

    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, SliderUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existedSlider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (existedSlider is null)
            return NotFound();


        if (vm.Image is not null)
        {
            if (!vm.Image.CheckFileSize(3000))
            {
                ModelState.AddModelError("ProductImages", "Sekil olcusu maximum 3 mb olmalidir");
                return View(vm);
            }

            if (!vm.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("ProductImages", "Mutleq shekil olmalidir!!!");
                return View(vm);
            }
        }

        existedSlider.Title = vm.Title;
        existedSlider.Subtitle = vm.Subtitle;
        existedSlider.Description = vm.Description;

        if (vm.Image is not null)
        {

            var removePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existedSlider.ImagePath);

            if (System.IO.File.Exists(removePath))
            {
                System.IO.File.Delete(removePath);
            }


            string fileName = $"{Guid.NewGuid()}-{vm.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await vm.Image.CopyToAsync(stream);
            }

            existedSlider.ImagePath = fileName;
        }

        _context.Update(existedSlider);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

        if (slider is null)
            return NotFound();

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();


        var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", slider.ImagePath);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }


        return RedirectToAction("Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Contexts;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class TopicController : Controller
{
    
        private readonly AppDbContext _context;

        public TopicController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var Topics = await _context.Topics.ToListAsync();
            return View(Topics);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TopicCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View();

            var isExist = await _context.Topics.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower());

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Topic artiq movcuddur");
                return View(vm);
            }


            Topic Topic = new() { Name = vm.Name };

            await _context.Topics.AddAsync(Topic);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var Topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id);

            if (Topic is null)
                return NotFound();

            TopicUpdateViewModel vm = new()
            {
                Name = Topic.Name,
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, TopicUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var existTopic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id);

            if (existTopic is null)
                return NotFound();

            var isExist = await _context.Topics.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Topic movcuddur");
                return View(vm);
            }


            existTopic.Name = vm.Name;

            _context.Topics.Update(existTopic);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }



        public async Task<IActionResult> Delete(int id)
        {
            var Topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id);

            if (Topic is null)
                return NotFound();

            _context.Topics.Remove(Topic);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


    }



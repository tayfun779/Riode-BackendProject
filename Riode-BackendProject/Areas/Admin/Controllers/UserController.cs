using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode_BackendProject.Areas.Admin.ViewModels;
using Riode_BackendProject.Models;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class UserController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();

        List<UserGetViewModel> userList = new();

        foreach (var user in users)
        {
            var roles=await _userManager.GetRolesAsync(user);

            UserGetViewModel userViewModel = new UserGetViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Fullname = user.Fullname,
                IsActive = user.IsActive,
                Role = roles.FirstOrDefault()
            };

            userList.Add(userViewModel);
        }

        return View(userList);
    }

    public async Task<IActionResult> ChangeStatus(string id)
    {
        var user=await _userManager.FindByIdAsync(id);

        if (user is null)
            return NotFound();


        var role = await _userManager.GetRolesAsync(user);

        if (role.FirstOrDefault() == "Admin")
        {
            return RedirectToAction("Index");
        }

        user.IsActive = !user.IsActive;

        await _userManager.UpdateAsync(user);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangeRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return NotFound();


        var role = await _userManager.GetRolesAsync(user);

        UserChangeRoleViewModel vm = new UserChangeRoleViewModel()
        {
            Username = user.UserName,
            Role = role.FirstOrDefault()
        };

        List<string> roles = new() { "Admin", "User", "Moderator" };

        ViewBag.Roles = roles;

        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(string id,UserChangeRoleViewModel vm)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return NotFound();

        var role = await _userManager.GetRolesAsync(user);

        await _userManager.AddToRoleAsync(user, vm.Role);

        await _userManager.RemoveFromRoleAsync(user, role.FirstOrDefault());



        return RedirectToAction("Index");

    }
}

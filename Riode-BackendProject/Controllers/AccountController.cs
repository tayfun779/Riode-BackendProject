using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode_BackendProject.Helpers;
using Riode_BackendProject.Models;
using Riode_BackendProject.ViewModels;

namespace Riode_BackendProject.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.FindByEmailAsync(vm.Email);

        if(user is null)
        {
            ModelState.AddModelError("", "Email ve ya sifre yanlisdir");
            return View(vm);
        }
        if (!user.IsActive)
        {
            ModelState.AddModelError("", "User block olunub");
            return View(vm);
        }
        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError("", "Emaili tesdiq edin");
            return View(vm);
        }

        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabiniz bloklanib 5 deqiqe sonra cehd edin");
                return View(vm);
            }

            ModelState.AddModelError("", "Email ve ya sifre yanlisdir");
            return View(vm);
        }

        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        AppUser user = new()
        {
            UserName = vm.Username,
            Email = vm.Email,
            Fullname = vm.Fullname,
            IsActive=true
        };


        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(vm);
            }
        }

        await _userManager.AddToRoleAsync(user, "Member");



        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        string link = Url.Action("ConfirmEmail", "Account", new { email = user.Email, token }, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);

        string body = $"<a href='{link}'>Confirm your email</a>";

        EmailHelper emailHelper = new EmailHelper(_configuration);
        await emailHelper.SendEmailAsync(new MailRequest { ToEmail = user.Email, Subject = "Confirm Email", Body = body });


        return RedirectToAction("Index", "Home");

    }


    public async Task<IActionResult> ConfirmEmail(string email,string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        if (await _userManager.IsEmailConfirmedAsync(user))
            return BadRequest();

        IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, token);
        if (identityResult.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }

        return BadRequest(); 
    }

    public async Task<IActionResult> CreateRoles()
    {


        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
        await _roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });

        return Content("Butun rollar yarandi");
    }
}

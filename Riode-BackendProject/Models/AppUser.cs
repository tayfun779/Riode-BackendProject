using Microsoft.AspNetCore.Identity;

namespace Riode_BackendProject.Models;

public class AppUser:IdentityUser
{
    public string Fullname { get; set; } = null!;
    public bool IsActive { get; set; }
}

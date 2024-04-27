namespace Riode_BackendProject.Areas.Admin.ViewModels;

public class UserGetViewModel
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Fullname { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }
}


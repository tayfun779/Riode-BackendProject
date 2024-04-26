using System.ComponentModel.DataAnnotations;

namespace Riode_BackendProject.ViewModels;

public class LoginViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }

}

using Riode_BackendProject.Models;

namespace Riode_BackendProject.ViewModels;

public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Brand> Brands { get; set; } = new();

}

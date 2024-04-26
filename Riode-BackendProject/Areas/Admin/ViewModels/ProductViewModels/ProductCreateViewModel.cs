using Riode_BackendProject.Models;
using System.ComponentModel.DataAnnotations;

namespace Riode_BackendProject.Areas.Admin.ViewModels;

public class ProductCreateViewModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; } = 0;
    public int Count { get; set; } = 0;
    public int Rating { get; set; }
    [Required]
    public int? CategoryId { get; set; }
    [Required]
    public int? BrandId { get; set; }
    public string Material { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public int DiscountPercent { get; set; }
    public ICollection<IFormFile> ProductImages { get; set; } = null!;
}

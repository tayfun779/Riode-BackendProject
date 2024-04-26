using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Riode_BackendProject.Areas.Admin.ViewModels;

public class ProductUpdateViewModel
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
    public List<IFormFile> ProductImages { get; set; } = new();
}
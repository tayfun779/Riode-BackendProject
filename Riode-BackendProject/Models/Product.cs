namespace Riode_BackendProject.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }=null!;
    public decimal Price { get; set; } = 0;
    public int Count { get; set; } = 0;
    public int Rating { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }=null!;
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public string Material { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public int DiscountPercent { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }


}


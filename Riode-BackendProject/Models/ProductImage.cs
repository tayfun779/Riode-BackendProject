namespace Riode_BackendProject.Models;

public class ProductImage
{
    public int Id { get; set; }
    public string Path { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}


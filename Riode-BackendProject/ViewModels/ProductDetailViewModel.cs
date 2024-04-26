using Riode_BackendProject.Models;

namespace Riode_BackendProject.ViewModels;

public class ProductDetailViewModel
{
    public Product Product { get; set; } = null!;
    public List<Product> RelatedProducts{ get; set; } = null!;

}

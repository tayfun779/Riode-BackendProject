using Riode_BackendProject.Models;

namespace Riode_BackendProject.ViewModels;

public class HomeViewModel
{
    public List<Slider> Sliders{ get; set; }=new();
    public List<Service> Services{ get; set; }=new();
    public List<Category> Categories{ get; set; }=new();
    public List<Product> BestSellerProducts{ get; set; }=new();
    public List<Product> OurFeaturedProducts{ get; set; }=new();
    public List<Product> SaleProducts{ get; set; }=new();
    public List<Product> LatestProducts{ get; set; }=new();
    public List<Product> BestOfWeekProducts{ get; set; }=new();
    public List<Product> PopularProducts{ get; set; }=new();
    public List<Blog> Blogs{ get; set; }=new();

}

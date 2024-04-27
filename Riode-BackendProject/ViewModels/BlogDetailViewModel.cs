using Riode_BackendProject.Models;

namespace Riode_BackendProject.ViewModels;

public class BlogDetailViewModel
{
    public List<Topic> Topics { get; set; } = new();
    public List<Blog> RelatedBlogs { get; set; } = new();
    public Blog Blog { get; set; } = null!;

}
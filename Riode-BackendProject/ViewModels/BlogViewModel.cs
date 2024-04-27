using Riode_BackendProject.Models;

namespace Riode_BackendProject.ViewModels;

public class BlogViewModel
{
    public List<Blog> Blogs { get; set; } = new();
    public List<Topic> Topics { get; set; } = new();

}

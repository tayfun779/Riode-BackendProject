namespace Riode_BackendProject.Areas.Admin.ViewModels;

public class BlogCreateViewModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
    public string Author { get; set; } = null!;
    public ICollection<int> BlogTopicIds { get; set; } = new List<int>();
}

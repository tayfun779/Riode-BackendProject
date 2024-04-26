namespace Riode_BackendProject.Models;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime CreatedTime { get; set; } = DateTime.Now;

    public ICollection<BlogTopic> BlogTopics { get; set; } = new List<BlogTopic>();
}

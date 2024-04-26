namespace Riode_BackendProject.Models;

public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<BlogTopic> BlogTopics { get; set; } = new List<BlogTopic>();

}

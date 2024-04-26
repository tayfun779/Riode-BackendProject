namespace Riode_BackendProject.Areas.Admin.ViewModels;

public class SliderCreateViewModel
{
    public string Title { get; set; } = null!;
    public string Subtitle { get; set; } = null!;
    public string Description{ get; set; } = null!;
    public IFormFile Image{ get; set; } = null!;
}

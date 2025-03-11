using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Dto.Product;

public class ProductCreateEditDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int RatingId { get; set; }
}

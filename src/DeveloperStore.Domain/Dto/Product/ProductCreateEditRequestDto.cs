using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Domain.Dto.Rating;
using DeveloperStore.Domain.Entities;


namespace DeveloperStore.Domain.Dto.Product;

public class ProductCreateEditRequestDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public RatingDto Rating { get; set; } = default!;
}

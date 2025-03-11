using DeveloperStore.Domain.Dto.Base;
using DeveloperStore.Domain.Dto.Rating;

namespace DeveloperStore.Domain.Dto.Product;

public class ProductCompleteDto : SimpleDtoBase
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public RatingCompleteDto Rating { get; set; } = default!;
}


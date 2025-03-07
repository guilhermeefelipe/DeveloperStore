using DeveloperStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Domain.Entities;

public class Product : SimpleEntityBase
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int RatingId { get; set; }

    [ForeignKey(nameof(RatingId))]
    public Rating Rating { get; set; } = default!;
}
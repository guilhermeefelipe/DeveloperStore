using DeveloperStore.Domain.Dto.Base;
using System.Text.Json.Serialization;

namespace DeveloperStore.Domain.Dto.Rating;

public class RatingCompleteDto
{
    [JsonIgnore] 
    public int Id { get; set; }
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
using DeveloperStore.Domain.Entities.Base;

namespace DeveloperStore.Domain.Entities;

public class Rating : SimpleEntityBase
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
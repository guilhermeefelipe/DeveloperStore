using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Domain.Entities.Base;

public abstract class SimpleEntityBase
{
    [Key]
    public int Id { get; set; }
}
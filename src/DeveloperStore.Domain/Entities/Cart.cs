using DeveloperStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Domain.Entities;

public class Cart : SimpleEntityBase
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!; 
    public string Date { get; set; } = string.Empty;

    [StringLength(int.MaxValue)]
    public string Products { get; set; } = string.Empty;
}
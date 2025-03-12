using DeveloperStore.Domain.Dto.Product;

namespace DeveloperStore.Domain.Dto.Cart;

public class CartCreateEditDto
{
    public int UserId { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Products { get; set; } = string.Empty;
}
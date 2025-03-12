using DeveloperStore.Domain.Dto.Base;
using DeveloperStore.Domain.Dto.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Domain.Dto.Cart;

public class CartDto : SimpleDtoBase
{
    public int UserId { get; set; }
    public string Date { get; set; } = string.Empty;

    public List<ProductCartDto> ProductList { get; set; } = default!;
}

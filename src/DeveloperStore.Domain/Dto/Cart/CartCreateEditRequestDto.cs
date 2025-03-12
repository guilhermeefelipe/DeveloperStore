using DeveloperStore.Domain.Dto.Base;
using DeveloperStore.Domain.Dto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Dto.Cart;

public class CartCreateEditRequestDto
{
    public int UserId { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<ProductCartDto> ProductsList { get; set; } = default!;
}
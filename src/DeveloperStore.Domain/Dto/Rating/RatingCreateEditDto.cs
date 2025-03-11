using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Dto.Rating;

public class RatingCreateEditDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Dto.Base;

public abstract class SimpleDtoBase
{
    public int Id { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Notes;

public class SimpleNote : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}

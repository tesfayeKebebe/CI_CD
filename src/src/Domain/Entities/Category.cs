using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category   : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public string LabId { get; set; } = null!;
        public Lab Lab { get; set; } = null!;
    }
}

using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LabTest  : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsFastingRequired { get; set; }
      
        
    }
}

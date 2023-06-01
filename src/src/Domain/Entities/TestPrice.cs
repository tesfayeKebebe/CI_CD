using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TestPrice  : BaseAuditableEntity
    {
        public double Price { get; set; }
        public string LabTestId { get; set; } = null!;
        public LabTest LabTest { get; set; } = null!;
    }
}

using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.LabTests
{
    public class LabTestCompletedEvent : BaseEvent
    {
        public LabTestCompletedEvent(LabTest item)
        {
            LabTest = item;
        }

        public LabTest LabTest { get; }
    }
}

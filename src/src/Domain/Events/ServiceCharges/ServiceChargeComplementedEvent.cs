using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.ServiceCharges
{
    public class ServiceChargeComplementedEvent : BaseEvent
    {
        public ServiceChargeComplementedEvent(ServiceCharge item)
        {
            ServiceCharge = item;
        }

        public ServiceCharge ServiceCharge { get; }
    }
}

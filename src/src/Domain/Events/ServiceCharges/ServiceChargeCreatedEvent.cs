using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.ServiceCharges
{
    public class ServiceChargeCreatedEvent : BaseEvent
    {
        public ServiceChargeCreatedEvent(ServiceCharge item)
        {
            ServiceCharge = item;
        }

        public ServiceCharge ServiceCharge { get; }
    }
}

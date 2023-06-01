using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.TestPrices
{
    public class TestPriceCompletedEvent : BaseEvent
    {
        public TestPriceCompletedEvent(TestPrice item)
        {
            TestPrice = item;
        }

        public TestPrice TestPrice { get; }
    }
}

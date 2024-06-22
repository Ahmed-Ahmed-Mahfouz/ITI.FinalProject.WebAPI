using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        New,
        pending ,
        representativeDelivered,
        delivered ,
        unreachable ,
        delayed,
        partialyDelivered,
        cancelled,
        rejectedWithPayment,
        rejectedWithPartialyPayment,
        rejectedWithoutPayment

    }
}

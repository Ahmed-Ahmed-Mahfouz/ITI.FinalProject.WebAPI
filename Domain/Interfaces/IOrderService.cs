using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderService
    {
        decimal CalculateDeliveryPrice(Order order, decimal baseDeliveryPrice, decimal weightMultiplier, bool isVillageDelivery, decimal shippingTypeMultiplier, decimal villageDeliveryFee, decimal baseWeight, decimal additionalFeePerKg);
        decimal CalculateCompanyProfit(Order order, decimal deductionValue, DeductionType deductionType);
    }
}

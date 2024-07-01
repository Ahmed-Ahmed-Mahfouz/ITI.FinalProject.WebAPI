using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        public decimal CalculateDeliveryPrice(Order order, decimal baseDeliveryPrice, decimal weightMultiplier, bool isVillageDelivery, decimal shippingTypeMultiplier, decimal villageDeliveryFee, decimal baseWeight, decimal additionalFeePerKg)
        {
            Settings settings = new Settings();
            City city = order.city;
            decimal deliveryPrice = city.normalShippingCost;
            decimal totalWeight = order.TotalWeight;

            if (totalWeight > settings.BaseWeight)
            {
                decimal additionalWeight = totalWeight - settings.BaseWeight;
                deliveryPrice += additionalWeight * settings.AdditionalFeePerKg;
            }

            switch(order.shipping.ShippingType)
            {
                case ShippingTypes.Ordinary:
                    deliveryPrice += settings.OrdinaryShippingCost;
                    break;
                case ShippingTypes.Within24Hours:
                    deliveryPrice += settings.TwentyFourHoursShippingCost;
                    break;
                case ShippingTypes.Within15Days:
                    deliveryPrice += settings.FifteenDayShippingCost;
                    break;
            }

            if (order.ShippingToVillage)
            {
                deliveryPrice += settings.VillageDeliveryFee;
            }

            return deliveryPrice;
        }

        public decimal CalculateCompanyProfit(Order order, decimal deductionValue, DeductionType deductionType)
        {
            decimal totalRevenue = order.OrderMoneyReceived ?? 0;
            decimal totalShippingRevenue = order.ShippingMoneyReceived ?? 0;
            decimal totalIncome = totalRevenue + totalShippingRevenue;

            decimal deductionAmount = deductionType == DeductionType.Amount ? deductionValue : totalIncome * (deductionValue / 100);

            return totalIncome - deductionAmount;
        }
    }
}

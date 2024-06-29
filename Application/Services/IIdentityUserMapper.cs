using Domain.Entities;

namespace Application.Services
{
    public interface IIdentityUserMapper
    {
            int? MapMerchantToId(Merchant merchant);
        

    }
}
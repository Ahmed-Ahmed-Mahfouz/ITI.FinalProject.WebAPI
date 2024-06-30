using Domain.Entities;

namespace Application.Services
{
    public interface IIdentityUserMapper
    {
            string? MapMerchantToId(Merchant merchant);
        

    }
}
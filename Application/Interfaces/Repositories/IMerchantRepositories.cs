using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMerchantRepositories
    {
        Task<IEnumerable<Merchant>> GetAllMerchantsAsync();
        Task<Merchant?> GetMerchantByEmailAsync(string email);
        Task<Merchant?> GetMerchantByIdAsync(int Merchant_id);
        Task DeleteMerchantAsync(Merchant Merchant);
        Task AddMerchantAsync(Merchant Merchant);
        Task UpdateAsync(Merchant Merchant);
        Task SaveChangesAsync();
        IQueryable<Merchant> GetMerchantsPaginated();
        Task<IEnumerable<Merchant>> GetFilteredMerchantsAsync(string searchSrting);
        Task<Merchant?> GetMerchantByUserIdAsync(string id);
    }
}

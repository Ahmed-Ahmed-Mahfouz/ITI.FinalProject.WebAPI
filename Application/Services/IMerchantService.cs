using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IMerchantService
    {
        Task<IEnumerable<MerchantResponseDto>?> GetAllMerchantsAsync();
        Task<MerchantResponseDto> GetMerchantByIdAsync(int id);
        Task<int?> GetMerchantIdByEmailAsync(string MerchantEmail);
        Task<List<ValidationResult>?> AddUserAndMerchant(MerchantAddDto MerchantAddDto);
        Task<List<ValidationResult>?> UpdateMerchantAsync(int Merchant_id, MerchantUpdateDto MerchantUpdateDto);
        Task<bool> DeleteMerchantAsync(int Merchant_id);
        IQueryable<MerchantResponseDto> GetMerchantsPaginated();
        Task<IEnumerable<MerchantResponseDto>> GetFilteredMerchantsAsync(string searchString);
    }
}

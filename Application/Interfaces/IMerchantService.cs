using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMerchantService: IGenericService<Merchant,MerchantResponseDto,MerchantAddDto,MerchantUpdateDto,string>
    {
        public new Task<List<ValidationResult>> UpdateObject(MerchantUpdateDto MerchantUpdateDto);
        public new Task<List<ValidationResult>> InsertObject(MerchantAddDto MerchantAddDto);

        public  Task<List<MerchantResponseDto>> GetFilteredMerchantsAsync(string searchString);
        public  Task<string?> GetMerchantIdByEmailAsync(string email, IIdentityUserMapper identityUserMapper);
        public Task<string?> GetMerchantIdByEmailAsync(string MerchantEmail);
    }
}

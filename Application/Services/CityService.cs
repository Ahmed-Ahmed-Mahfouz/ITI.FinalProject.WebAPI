using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Domain.Services
{
    public class CityService : IGenericService<City,CityDisplayDTO,CityInsertDTO,CityUpdateDTO,int>
    {
        public IGenericRepository<City> CityRepo;
        public CityService( IUnitOfWork unit)
        {
            CityRepo= unit.GetGenericRepository<City>(); 
        }
        public async Task<bool> DeleteObject(int ObjectId)
        {
            City? City = await CityRepo.GetElement(b => b.id == ObjectId);
            if(City == null)
            {
                return false;
            }

            return CityRepo.Delete(City);
        }

        public async Task<List<CityDisplayDTO>> GetAllObjects()
        {
            List<City>? Cityes = await CityRepo.GetAllElements();
            if(Cityes == null)
            {
                return null;
            }
            List<CityDisplayDTO> CitysDTO = new List<CityDisplayDTO>();
            foreach (var item in Cityes)
            {
                CitysDTO.Add(
                    new CityDisplayDTO
                    {
                        name = item.name,
                        status = item.status,
                        branchName= item.branch.name,
                        cityMerchants = item.cityMerchants,
                        normalShippingCost = item.normalShippingCost,
                        pickupShippingCost = item.pickupShippingCost,
                        cityOrders = item.cityOrders,
                        stateId = item.stateId
                    }
                );    
            }
            return CitysDTO;
        }

        public  async Task<List<CityDisplayDTO>> GetAllObjects(params Expression<Func<City, object>>[] includes)
        {
            List<City>? Cityes = await CityRepo.GetAllElements(includes);
            if (Cityes == null)
            {
                return null;
            }
            List<CityDisplayDTO> CitysDTO = new List<CityDisplayDTO>();
            foreach (var item in Cityes)
            {
                CitysDTO.Add(
                    new CityDisplayDTO
                    {
                        name = item.name,
                        status = item.status,
                        branchName = item.branch.name,
                        cityMerchants = item.cityMerchants,
                        normalShippingCost = item.normalShippingCost,
                        pickupShippingCost = item.pickupShippingCost,
                        cityOrders = item.cityOrders,
                        stateId = item.stateId
                    }
                );
            }
            return CitysDTO;
        }

        public async Task<CityDisplayDTO?> GetObject(Expression<Func<City, bool>> filter)
        {
            City?  City = await CityRepo.GetElement(filter);
            if (City == null)
            {
                return null;
            }
            CityDisplayDTO CityDTO = new CityDisplayDTO(){
                name = City.name,
                status = City.status,
                branchName = City.branch.name,
                cityMerchants = City.cityMerchants,
                normalShippingCost = City.normalShippingCost,
                pickupShippingCost = City.pickupShippingCost,
                cityOrders = City.cityOrders,
                stateId = City.stateId
            };
            
            return CityDTO;
        }

       

        public async Task<CityDisplayDTO?> GetObject(Expression<Func<City, bool>> filter, params Expression<Func<City, object>>[] includes)
        {
            City? City = await CityRepo.GetElement(filter, includes);
            if (City == null)
            {
                return null;
            }
            CityDisplayDTO CityDTO = new CityDisplayDTO()
            {
                name = City.name,
                status = City.status,
                branchName = City.branch.name,
                cityMerchants = City.cityMerchants,
                normalShippingCost = City.normalShippingCost,
                pickupShippingCost = City.pickupShippingCost,
                cityOrders = City.cityOrders,
                stateId = City.stateId
            };

            return CityDTO;
        }

        public async Task<CityDisplayDTO?> GetObjectWithoutTracking(Expression<Func<City, bool>> filter)
        {
            City? City = await CityRepo.GetElementWithoutTracking(filter);
            if (City == null)
            {
                return null;
            }
            CityDisplayDTO CityDTO = new CityDisplayDTO()
            {
                name = City.name,
                status = City.status,
                branchName = City.branch.name,
                cityMerchants = City.cityMerchants,
                normalShippingCost = City.normalShippingCost,
                pickupShippingCost = City.pickupShippingCost,
                cityOrders = City.cityOrders,
                stateId = City.stateId
            };

            return CityDTO;
        }


        public async Task<CityDisplayDTO?> GetObjectWithoutTracking(Expression<Func<City, bool>> filter, params Expression<Func<City, object>>[] includes)
        {
            City? City = await CityRepo.GetElementWithoutTracking(filter,includes);
            if (City == null)
            {
                return null;
            }
            CityDisplayDTO CityDTO = new CityDisplayDTO()
            {
                name = City.name,
                status = City.status,
                branchName = City.branch.name,
                cityMerchants = City.cityMerchants,
                normalShippingCost = City.normalShippingCost,
                pickupShippingCost = City.pickupShippingCost,
                cityOrders = City.cityOrders,
                stateId = City.stateId
            };

            return CityDTO;
        }

        public async Task<bool> InsertObject(CityInsertDTO ObjectDTO)
        {
            City City = new City() {
                   id = 0,
                    name = ObjectDTO.name,
                    status = ObjectDTO.status,
                    normalShippingCost = ObjectDTO.normalShippingCost,
                    pickupShippingCost = ObjectDTO.pickupShippingCost,
                    stateId = ObjectDTO.stateId

            };
            return CityRepo.Add(City);

        }

        public Task<bool> SaveChangesForObject()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateObject(CityUpdateDTO ObjectDTO)
        {
            //City? City = CityRepo.GetElement(b => b.id == ObjectDTO.id);
            //if(City == null)
            //{
            //    return false;
            //}
            City City = new City();
            City.id = ObjectDTO.id;
            City.name = ObjectDTO.name;
            City.normalShippingCost = ObjectDTO.normalShippingCost;
            City.pickupShippingCost = ObjectDTO.pickupShippingCost;
            City.status = ObjectDTO.status;
            
            
            return CityRepo.Edit(City);
        }

        
    }
}

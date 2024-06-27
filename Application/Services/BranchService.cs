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
    public class BranchService : IGenericService<Branch,BranchDisplayDTO,BranchInsertDTO,BranchUpdateDTO>
    {
        public IGenericRepository<Branch> branchRepo;
        public BranchService( IUnitOfWork unit)
        {
            branchRepo= unit.GetGenericRepository<Branch>(); 
        }
       
        public async Task<List<BranchDisplayDTO>> GetAllObjects()
        {
            List<Branch>? branches= await branchRepo.GetAllElements();
            if(branches == null)
            {
                return null;
            }
            List<BranchDisplayDTO> branchsDTO = new List<BranchDisplayDTO>();
            foreach (var item in branches)
            {
                branchsDTO.Add(
                    new BranchDisplayDTO
                    {
                        name = item.name,
                        addingDate = item.addingDate,
                        cityId = item.cityId,
                        status = item.status
                    }
                );    
            }
            return branchsDTO;
        }

        public  async Task<List<BranchDisplayDTO>> GetAllObjects(params Expression<Func<Branch, object>>[] includes)
        {
            List<Branch>? branches = await branchRepo.GetAllElements(includes);
            if (branches == null)
            {
                return null;
            }
            List<BranchDisplayDTO> branchsDTO = new List<BranchDisplayDTO>();
            foreach (var item in branches)
            {
                branchsDTO.Add(
                    new BranchDisplayDTO
                    {
                        name = item.name,
                        addingDate = item.addingDate,
                        cityId = item.cityId,
                        status = item.status
                    }
                );
            }
            return branchsDTO;
        }

        public async Task<BranchDisplayDTO?> GetObject(Expression<Func<Branch, bool>> filter)
        {
            Branch?  branch = await branchRepo.GetElement(filter);
            if (branch == null)
            {
                return null;
            }
            BranchDisplayDTO branchDTO = new BranchDisplayDTO(){
                name = branch.name,
                addingDate = branch.addingDate,
                cityId = branch.cityId,
                status = branch.status
            };
            
            return branchDTO;
        }

       

        public async Task<BranchDisplayDTO?> GetObject(Expression<Func<Branch, bool>> filter, params Expression<Func<Branch, object>>[] includes)
        {
            Branch? branch = await branchRepo.GetElement(filter, includes);
            if (branch == null)
            {
                return null;
            }
            BranchDisplayDTO branchDTO = new BranchDisplayDTO()
            {
                name = branch.name,
                addingDate = branch.addingDate,
                cityId = branch.cityId,
                status = branch.status
            };

            return branchDTO;
        }

        public async Task<BranchDisplayDTO?> GetObjectWithoutTracking(Expression<Func<Branch, bool>> filter)
        {
            Branch? branch = await branchRepo.GetElementWithoutTracking(filter);
            if (branch == null)
            {
                return null;
            }
            BranchDisplayDTO branchDTO = new BranchDisplayDTO()
            {
                name = branch.name,
                addingDate = branch.addingDate,
                cityId = branch.cityId,
                status = branch.status
            };

            return branchDTO;
        }


        public async Task<BranchDisplayDTO?> GetObjectWithoutTracking(Expression<Func<Branch, bool>> filter, params Expression<Func<Branch, object>>[] includes)
        {
            Branch? branch = await branchRepo.GetElementWithoutTracking(filter,includes);
            if (branch == null)
            {
                return null;
            }
            BranchDisplayDTO branchDTO = new BranchDisplayDTO()
            {
                name = branch.name,
                addingDate = branch.addingDate,
                cityId = branch.cityId,
                status = branch.status
            };

            return branchDTO;
        }

        public bool InsertObject(BranchInsertDTO ObjectDTO)
        {
            Branch branch = new Branch() {
                   id = 0,
                   name = ObjectDTO.name,
                   addingDate = ObjectDTO.addingDate,
                   cityId = ObjectDTO.cityId,
                   status = ObjectDTO.status
                   
            };
            return branchRepo.Add(branch);

        }

        public Task<bool> SaveChangesForObject()
        {
            throw new NotImplementedException();
        }

        public bool UpdateObject(BranchUpdateDTO ObjectDTO)
        {
            //Branch? branch = branchRepo.GetElement(b => b.id == ObjectDTO.id);
            //if(branch == null)
            //{
            //    return false;
            //}
            Branch branch = new Branch();
            branch.id = ObjectDTO.id;
            branch.name = ObjectDTO.name;
            branch.addingDate = ObjectDTO.addingDate;
            branch.cityId = ObjectDTO.cityId;
            branch.status = ObjectDTO.status;
            
            return branchRepo.Edit(branch);
        }
        public async Task<bool> DeleteObject(int ObjectId)
        {
            Branch? branch = await branchRepo.GetElement(b => b.id == ObjectId);
            if (branch == null)
            {
                return false;
            }

            return branchRepo.Delete(branch);
        }



    }
}

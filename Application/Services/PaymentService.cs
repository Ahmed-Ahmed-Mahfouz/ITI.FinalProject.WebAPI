using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class PaymentService : IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Payment> _repository;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetGenericRepository<Payment>();
        }

        public async Task<List<DisplayPaymentDTO>> GetAllObjects()
        {
            var payments = await _repository.GetAllElements();
            return _mapper.Map<List<DisplayPaymentDTO>>(payments);
        }

        public async Task<List<DisplayPaymentDTO>> GetAllObjects(params System.Linq.Expressions.Expression<Func<Payment, object>>[] includes)
        {
            var payments = await _repository.GetAllElements(includes);
            return _mapper.Map<List<DisplayPaymentDTO>>(payments);
        }

        public async Task<DisplayPaymentDTO?> GetObject(System.Linq.Expressions.Expression<Func<Payment, bool>> filter)
        {
            var payment = await _repository.GetElement(filter);
            return _mapper.Map<DisplayPaymentDTO>(payment);
        }

        public async Task<DisplayPaymentDTO?> GetObject(System.Linq.Expressions.Expression<Func<Payment, bool>> filter, params System.Linq.Expressions.Expression<Func<Payment, object>>[] includes)
        {
            var payment = await _repository.GetElement(filter, includes);
            return _mapper.Map<DisplayPaymentDTO>(payment);
        }

        public async Task<DisplayPaymentDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Payment, bool>> filter)
        {
            var payment = await _repository.GetElementWithoutTracking(filter);
            return _mapper.Map<DisplayPaymentDTO>(payment);
        }

        public async Task<DisplayPaymentDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Payment, bool>> filter, params System.Linq.Expressions.Expression<Func<Payment, object>>[] includes)
        {
            var payment = await _repository.GetElementWithoutTracking(filter, includes);
            return _mapper.Map<DisplayPaymentDTO>(payment);
        }

        public async Task<bool> InsertObject(InsertPaymentDTO paymentDTO)
        {
            var payment = _mapper.Map<Payment>(paymentDTO);
            _repository.Add(payment);
            return true;
        }

        public async Task<bool> UpdateObject(UpdatePaymentDTO paymentDTO)
        {
            var payment = _mapper.Map<Payment>(paymentDTO);
            _repository.Edit(payment);
            return true;
        }

        //public bool InsertObject(InsertPaymentDTO paymentDTO)
        //{
        //    var payment = _mapper.Map<Payment>(paymentDTO);
        //    _repository.Add(payment);
        //    return true;
        //}

        //public bool UpdateObject(UpdatePaymentDTO paymentDTO)
        //{
        //    var payment = _mapper.Map<Payment>(paymentDTO);
        //    _repository.Edit(payment);
        //    return true;
        //}

        public async Task<bool> DeleteObject(int paymentId)
        {
            var payment = await _repository.GetElement(x => x.id == paymentId);
            _repository.Delete(payment);
            return true;
        }

        public async Task<bool> SaveChangesForObject()
        {
            return await _unitOfWork.SaveChanges();
        }
    }
}

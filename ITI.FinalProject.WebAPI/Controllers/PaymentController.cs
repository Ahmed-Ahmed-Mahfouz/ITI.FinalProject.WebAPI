using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO, int> _paymentService;
        public PaymentController(IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO, int> paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayPaymentDTO>>> GetPayments()
        {
            var payments = await _paymentService.GetAllObjects();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayPaymentDTO>> GetPayment(int id)
        {
            var payment = await _paymentService.GetObject(p => p.id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpPost]
        public async Task<IActionResult> PostPayment(InsertPaymentDTO paymentDTO)
        {
            var success = await _paymentService.InsertObject(paymentDTO);

            if (!success)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, UpdatePaymentDTO paymentDTO)
        {
            if (id != paymentDTO.Id)
            {
                return BadRequest();
            }

            var success = await _paymentService.UpdateObject(paymentDTO);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var success = await _paymentService.DeleteObject(id);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}

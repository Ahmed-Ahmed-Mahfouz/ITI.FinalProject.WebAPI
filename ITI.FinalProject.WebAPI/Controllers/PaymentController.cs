using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        // GET: api/Payment
        [SwaggerOperation(Summary = "This Endpoint returns a list of payments", Description = "")]
        [SwaggerResponse(404, "There weren't any payments in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of payments", Type = typeof(List<DisplayPaymentDTO>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayPaymentDTO>>> GetPayments()
        {
            var payments = await _paymentService.GetAllObjects();

            if (payments == null || payments.Count == 0)
            {
                return NotFound();
            }

            return Ok(payments);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "This Endpoint returns the specified payment")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified payment", Type = typeof(DisplayPaymentDTO))]
        public async Task<ActionResult<DisplayPaymentDTO>> GetPayment(int id)
        {
            var payment = await _paymentService.GetObject(p => p.id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // POST: api/Payment
        [SwaggerOperation(Summary = "This Endpoint inserts a new payment in the db", Description = "")]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the payment was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostPayment(InsertPaymentDTO paymentDTO)
        {
            if (await _paymentService.InsertObject(paymentDTO))
            {
                if (await _paymentService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // PUT: api/Payment/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified payment", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given payment object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the payment was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, UpdatePaymentDTO paymentDTO)
        {
            if (id != paymentDTO.Id)
            {
                return BadRequest();
            }

            var success = await _paymentService.GetObjectWithoutTracking(p => p.id == id);

            if (success == null)
            {
                return NotFound();
            }

            if (await _paymentService.UpdateObject(paymentDTO))
            {
                if (await _paymentService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // DELETE: api/Payment/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified payment", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the payment was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var success = await _paymentService.GetObjectWithoutTracking(p => p.id == id);

            if (success == null)
            {
                return NotFound();
            }

            if (await _paymentService.DeleteObject(id))
            {
                if (await _paymentService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }
    }
}

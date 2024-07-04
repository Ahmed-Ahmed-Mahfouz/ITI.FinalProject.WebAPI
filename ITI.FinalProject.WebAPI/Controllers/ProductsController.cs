using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int> _productService;
        public ProductsController(IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int> productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [SwaggerOperation(Summary = "This Endpoint returns a list of products", Description = "")]
        [SwaggerResponse(404, "There weren't any products in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of products", Type = typeof(List<DisplayProductDTO>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayProductDTO>>> GetProducts()
        {
            var products = await _productService.GetAllObjects();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // GET: api/Products/5
        [SwaggerOperation(Summary = "This Endpoint returns the specified product")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified product", Type = typeof(DisplayProductDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetObject(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [SwaggerOperation(Summary = "This Endpoint inserts a new product in the db", Description = "")]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the product was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostProduct(InsertProductDTO productDTO)
        {
            var result = await _productService.InsertObject(productDTO);

            if (result.Succeeded)
            {
                if (await _productService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }

        // PUT: api/Products/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified product", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given product object", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the product was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var success = await _productService.GetObjectWithoutTracking(p => p.Id == id);

            if (success == null)
            {
                return NotFound("Product doesn't exist in the db");
            }

            var result = await _productService.UpdateObject(productDTO);

            if (result.Succeeded)
            {
                if (await _productService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }

        // DELETE: api/Products/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified product", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the product was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.GetObjectWithoutTracking(p => p.Id == id);

            if (success == null)
            {
                return NotFound("Product doesn't exist in the db");
            }

            var result = await _productService.DeleteObject(id);

            if (result.Succeeded)
            {
                if (await _productService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }
    }
}

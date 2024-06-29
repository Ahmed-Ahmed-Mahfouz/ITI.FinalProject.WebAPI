using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int> _productService;
        public ProductsController(IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayProductDTO>>> GetProducts()
        {
            var products = await _productService.GetAllObjects();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetObject(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(InsertProductDTO productDTO)
        {
            var success = await _productService.InsertObject(productDTO);

            if (!success)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            var success = await _productService.UpdateObject(productDTO);

            if (!success)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteObject(id);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int page, string productName)
        {
            if (!string.IsNullOrEmpty(productName))
            {
                var filtered = await _repository.GetFilteredProduct(productName);
                return Ok(filtered);
            }
            else if (page <= 0)
            {
                _logger.LogInformation("Getting all products");
                return Ok(await _repository.GetProducts());
            }
            else
            {
                _logger.LogInformation("Getting products per page " + page);
                return Ok(await _repository.GetProductByPage(page, 6));
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogWarning("Didn't find product with id " + id);
                return NotFound();
            }
            _logger.LogInformation("Found product with id " + id);
            return Ok(product);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var categories = await _repository.GetProductByCategory(category);
            if (categories == null)
            {
                _logger.LogWarning("Didn't find products with category " + category);
                return NotFound();
            }
            _logger.LogInformation("Found products with category " + category);
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.Create(product);
            return Created(new Uri(Request.GetEncodedUrl()), product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product value)
        {
            if (await _repository.Update(value))
            {
                _logger.LogInformation("Updated product with id " + value.Id);
                return Ok(value);
            }
            _logger.LogError("Failed to Update product with id " + value.Id);
            return BadRequest("Failed to update product");
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            if (await _repository.Delete(id))
            {
                _logger.LogInformation("Deleted product with id " + id);
                return Ok();
            }
            _logger.LogError("Failed to delete product with id " + id);
            return BadRequest("Failed to delete product");
        }
    }
}
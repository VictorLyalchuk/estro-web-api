using Core.DTOs.Filter;
using Core.DTOs.Product;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _product;
        public ProductController(IProductService product)
        {
            _product = product;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _product.GetProductByCategoryIdAsync(id));
        }

        [HttpGet("ProductByID/{id}")]
        public async Task<IActionResult> ProductByIDAsync(int id)
        {
            var product = await _product.GetProductByIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("ProductByUrlName/{urlName}")]
        public async Task<IActionResult> GetProductByURLNameAsync([FromRoute] string urlName)
        {
            var products = await _product.GetProductByURLNameAsync(urlName);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("ProductQuantityByFilters")]
        public async Task<IActionResult> ProductQuantityByFiltersAsync([FromQuery] FilterDTO? filterDTO)
        {
            var quantity = await _product.ProductQuantityByFiltersAsync(filterDTO);
            return Ok(quantity);
        }

        [HttpGet("FilterProducts")]
        public async Task<IActionResult> FilterProductsAsync([FromQuery] FilterDTO? filterDTO)
        {
            var products = await _product.FilterProductsAsync(filterDTO);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        //[HttpGet("FilterProductsByPage")]
        //public async Task<IActionResult> FilterProductsByPage(int page, string? search)
        //{
        //    var product = await _product.GetProductByPageAsync(page, search);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}

        //CRUD Admin
        [HttpGet("ProductByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ProductByPageAsync(int page, string? search)
        {
            var product = await _product.GetProductByPageAsync(page, search);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("GetEditProductById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetEditProductById(int id)
        {
            var product = await _product.GetEditProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("ProductQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ProductQuantity(string? search)
        {
            var quantity = await _product.ProductQuantity(search);
            return Ok(quantity);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _product.CreateAsync(createProductDTO);
            return Ok();
        }

        [HttpPost("EditProduct")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditProduct(EditProductDTO editProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _product.EditAsync(editProductDTO);
            return Ok();
        }

        [HttpDelete("DeleteProductByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProductByIDAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _product.DeleteProductByIDAsync(id);
            return Ok();
        }
    }
}

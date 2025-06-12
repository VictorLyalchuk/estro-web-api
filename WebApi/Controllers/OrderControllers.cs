using Core.DTOs.UserInfo;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderControllers : Controller
    {
        private readonly IOrderService _order;
        public OrderControllers(IOrderService order)
        {
            _order = order;
        }
        
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateProductAsync(OrderCreateDTO orderCreateDTO)
        {
            await _order.CreateAsync(orderCreateDTO);
            return Ok();
        }
        [HttpGet("GetTopPopularProducts")]
        public async Task<IActionResult> GetTopPopularProducts()
        {
            var topProducts = await _order.GetTopPopularProductsAsync();
            return Ok(new { value = topProducts });
        }
        [HttpGet("GetTopFourCategories")]
        public async Task<ActionResult<List<CategoryDistributionDTO>>> GetTopFourCategories()
        {
            try
            {
                var topCategories = await _order.GetPopularCategoriesAsync();
                return Ok(topCategories);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetGenderPercentage")]
        public async Task<IActionResult> GetGenderDataForChart()
        {

            var result = await _order.GetGenderDataForChartAsync();

            var response = new GenderDataResponse
            {
                WomenCount = result.WomenCount,
                MenCount = result.MenCount,
                WomenPercentage = result.WomenPercentage,
                MenPercentage = result.MenPercentage
            };

            return Ok(response);
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var result = await _order.GetAllOrdersAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return Ok();
        }
        [HttpGet("GetTotalByMonth")]
        public async Task<ActionResult<decimal>> GetMonthlyOrderTotal(int month)
        {
            var result = await _order.GetMonthlyOrderTotal(month);
            return result;
        }
        [HttpGet("GetTotalByDay")]
        public async Task<ActionResult<DailyOrderTotalDTO>> GetOrderTotalsForSpecificDay(int day)
        {
            var result = await _order.GetOrderTotalForSpecificDay(day);
            return result; // This returns the ActionResult that already has Ok() inside it
        }
        [HttpGet("GetTotalByWeek")]
        public async Task<IActionResult> GetDailyOrderTotal([FromQuery] string week, [FromQuery] int day)
        {
            if (string.IsNullOrEmpty(week) || day < 1 || day > 7)
                return BadRequest("Invalid parameters. Week should be 'current' or 'previous', and day should be between 1 and 7.");

            try
            {
                var totalAmount = await _order.GetOrderTotalForDayAsync(week, day);
                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetOrderById/{Id}")]
        public async Task<IActionResult> GetOrderByIdAsync(string Id, int page)
        {
            var result = await _order.GetOrderByIdAsync(Id, page);
            if (result != null)
            {
                return Ok(result);
            }
            return Ok();
        }
        
        [HttpGet("GetCountOrderById/{Id}")]
        public async Task<IActionResult> GetCountOrderByIdAsync(string Id)
        {
            var result = await _order.GetCountOrderByIdAsync(Id);
            if (result != null)
            {
                return Ok(result);
            }
            return Ok();
        }

        // CRUD Store
        [HttpGet("OrderByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OrderByPageAsync(int page, [FromQuery] int pageSize, string step)
        {
            var stepArray = JsonConvert.DeserializeObject<int[]>(step);
            var orders = await _order.OrderByPageAsync(page, pageSize, stepArray);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("OrderQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OrderQuantityAsync([FromQuery] string step)
        {
            var stepArray = JsonConvert.DeserializeObject<int[]>(step);
            var quantity = await _order.OrderQuantityAsync(stepArray);
            return Ok(quantity);
        }

        [HttpPost("EditOrderItems")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditOrderItemsAsync(OrderItemsDTO orderItemsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _order.EditOrderItemsAsync(orderItemsDTO);
            return Ok();
        }
    }
}

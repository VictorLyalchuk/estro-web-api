using Core.DTOs.UserInfo;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task CreateAsync(OrderCreateDTO orderCreateDTO);
        Task <List<OrderDTO>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetOrderByIdAsync(string Id, int page);
        Task<int> GetCountOrderByIdAsync(string Id);
        Task<List<OrderItemsDTO>> OrderByPageAsync(int page, int pageSize, int[] step);
        Task<int> OrderQuantityAsync(int[] step);
        Task EditOrderItemsAsync(OrderItemsDTO orderItemsDTO);
        Task<ActionResult<DailyOrderTotalDTO>> GetOrderTotalForSpecificDay(int day);
        Task<ActionResult<decimal>> GetMonthlyOrderTotal(int month);
        Task<decimal> GetOrderTotalForDayAsync(string week, int day);
        Task<(int WomenCount, int MenCount, double WomenPercentage, double MenPercentage)> GetGenderDataForChartAsync();
        Task<IActionResult> GetPopularCategoriesAsync();
        Task<List<PopularProductDTO>> GetTopPopularProductsAsync();



    }
}

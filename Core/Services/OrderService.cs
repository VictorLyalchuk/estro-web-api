using AutoMapper;
using Core.DTOs.Product;
using Core.DTOs.UserInfo;
using Core.Entities.Address;
using Core.Entities.Category;
using Core.Entities.Product;
using Core.Entities.UserEntity;
using Core.Entities.UserInfo;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Core.Specification.OrderSpecification;

namespace Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<SubCategory> _subcategoryRepository;
        private readonly IRepository<MainCategory> _mainCategoryRepository;

        private readonly IRepository<OrderPayment> _orderPaymentRepository;
        private readonly IRepository<OrderItems> _orderItemsRepository;
        private readonly IRepository<Bag> _bagRepository;
        private readonly IRepository<BagItems> _bagItemsRepository;
        private readonly IRepository<AddressEntity> _addressRepository;
        private readonly IRepository<UserBonuses> _userBonuses;
        private readonly UserManager<User> _userManager;
        private readonly IStorageService _storageService;
        public OrderService(IRepository<CategoryEntity> categoryRepository,
            IRepository<SubCategory> subcategoryRepository,
            IRepository<MainCategory> mainCategoryRepository, IRepository<ProductEntity> productRepository, IRepository<Order> repositoryRepository, IMapper mapper, IRepository<Bag> bagRepository, UserManager<User> userManager, IRepository<BagItems> bagItemsRepository, IRepository<OrderItems> orderItemsRepository, IRepository<AddressEntity> addressRepository, IRepository<UserBonuses> userBonuses, IRepository<OrderPayment> orderPaymentRepository, IStorageService storageService)
        {
            _mapper = mapper;
            _orderRepository = repositoryRepository;
            _bagRepository = bagRepository;
            _userManager = userManager;
            _bagItemsRepository = bagItemsRepository;
            _orderItemsRepository = orderItemsRepository;
            _addressRepository = addressRepository;
            _userBonuses = userBonuses;
            _orderPaymentRepository = orderPaymentRepository;
            _storageService = storageService;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
            _mainCategoryRepository = mainCategoryRepository;
        }
        public async Task CreateAsync(OrderCreateDTO orderCreateDTO)
        {
            var user = await _userManager.FindByEmailAsync(orderCreateDTO.EmailUser);
            var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByUserEmail(orderCreateDTO.EmailUser));
            var bagItems = await _bagItemsRepository.GetListBySpec(new BagItemsSpecification.GetBagItemsByBagEmail(orderCreateDTO.EmailUser));

            if (bag != null)
            {
                var address = await CreateAddressAsync(orderCreateDTO);


                var (total, tax, subtotal, accrued) = CalculateOrderTotals(bagItems, orderCreateDTO.Discount);

                var order = await CreateOrderAsync(orderCreateDTO, address.Id, user.Id, total, tax, subtotal);

                if (user != null)
                {
                    await UpdateUserAsync(user, orderCreateDTO.Discount, accrued);
                }

                await CreateOrderItemsAsync(bagItems, order.Id, orderCreateDTO);
                await ClearBagAsync(bag.Id);

                await CreateUserBonusesAsync(user.Id, order.Id, orderCreateDTO.Discount, accrued);
            }
        }

        public async Task<List<PopularProductDTO>> GetTopPopularProductsAsync()
        {
            var orderItems = await _orderItemsRepository.GetAsync(
                includeProperties: "Product"
            );

            var totalCount = orderItems.Count();

            var productCounts = orderItems
                .GroupBy(o => new
                {
                    o.Product.Name_en,
                    o.Product.Name_fr,
                    o.Product.Name_es,
                    o.Product.Name_uk
                })
                .Select(g => new
                {
                    Name_en = g.Key.Name_en,
                    Name_fr = g.Key.Name_fr,
                    Name_es = g.Key.Name_es,
                    Name_uk = g.Key.Name_uk,
                    Count = g.Count(),
                    Percentage = g.Count() / (double)totalCount * 100
                })
                .OrderByDescending(p => p.Count)
                .ToList();

            // Get top 4 products
            var topProducts = productCounts.Take(4).ToList();

            // Calculate "Other" category
            var otherCategory = new
            {
                Name_en = "Other",
                Name_fr = "Autre",
                Name_es = "Otro",
                Name_uk = "Інше",
                Count = productCounts.Skip(4).Sum(p => p.Count),
                Percentage = productCounts.Skip(4).Sum(p => p.Percentage)
            };

            // Add "Other" category to the list
            topProducts.Add(otherCategory);

            // Convert to the DTO
            return topProducts.Select(p => new PopularProductDTO
            {
                ProductName_en = p.Name_en,
                ProductName_fr = p.Name_fr,
                ProductName_es = p.Name_es,
                ProductName_uk = p.Name_uk,
                Count = p.Count,
                Percentage = p.Percentage
            }).ToList();
        }

        public async Task<IActionResult> GetPopularCategoriesAsync()
        {
            // Fetch all order items with product and category information
            var orderItems = await _orderItemsRepository.GetAsync(
                includeProperties: "Product.Category.SubCategory.MainCategory"
            );

            // Group by category ID and count the occurrences
            var categoryCounts = orderItems
                .GroupBy(item => item.Product.Category.Id)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    Count = group.Count()
                })
                .ToList();

            // Get the top 4 categories
            var topCategories = categoryCounts
                .OrderByDescending(c => c.Count)
                .Take(4)
                .ToList();

            // Get the remaining categories for the "Other" category
            var remainingCategories = categoryCounts
                .Except(topCategories)
                .Sum(c => c.Count);

            var result = new List<CategoryDistributionDTO>();

            // Add the top 4 categories to the result
            foreach (var category in topCategories)
            {
                var categoryEntity = orderItems
                    .Select(item => item.Product.Category)
                    .FirstOrDefault(cat => cat.Id == category.CategoryId);

                result.Add(new CategoryDistributionDTO
                {
                    Name_en = categoryEntity?.Name_en ?? "Unknown",
                    Name_fr = categoryEntity?.Name_fr ?? "Unknown",
                    Name_es = categoryEntity?.Name_es ?? "Unknown",
                    Name_uk = categoryEntity?.Name_uk ?? "Unknown",
                    Count = category.Count,
                    Percentage = category.Count * 100.0 / orderItems.Count()
                });
            }

            // Add the "Other" category
            result.Add(new CategoryDistributionDTO
            {
                Name_en = "Other",
                Name_fr = "Autre",
                Name_es = "Otro",
                Name_uk = "Інше",
                Count = remainingCategories,
                Percentage = remainingCategories * 100.0 / orderItems.Count()
            });

            return new OkObjectResult(result);
        }


        public async Task<(int WomenCount, int MenCount, double WomenPercentage, double MenPercentage)> GetGenderDataForChartAsync()
        {
            // Fetch all order items
            var orderItems = await _orderItemsRepository.GetAsync(
                includeProperties: "Product.Category.SubCategory.MainCategory"
            );

            int womenCount = 0;
            int menCount = 0;

            // Analyze order items
            foreach (var item in orderItems)
            {
                var product = item.Product;

                if (product != null)
                {
                    var mainCategory = product.Category?.SubCategory?.MainCategory;

                    if (mainCategory != null)
                    {
                        // Check if the product's main category indicates "women" or "men"
                        if (mainCategory.URLName.Contains("women", StringComparison.OrdinalIgnoreCase))
                        {
                            womenCount++;
                        }
                        else if (mainCategory.URLName.Contains("men", StringComparison.OrdinalIgnoreCase))
                        {
                            menCount++;
                        }
                    }
                }
            }

            int totalCount = womenCount + menCount;
            double womenPercentage = totalCount > 0 ? (double)womenCount / totalCount * 100 : 0;
            double menPercentage = totalCount > 0 ? (double)menCount / totalCount * 100 : 0;

            return (womenCount, menCount, womenPercentage, menPercentage);
        }






        public async Task<decimal> GetOrderTotalForDayAsync(string week, int day)
        {
            DateTime startOfWeek;
            DateTime endOfWeek;

            if (week == "current")
            {
                startOfWeek = GetStartOfWeek(DateTime.Today);
                endOfWeek = startOfWeek.AddDays(6);
            }
            else if (week == "previous")
            {
                startOfWeek = GetStartOfWeek(DateTime.Today.AddDays(-7));
                endOfWeek = startOfWeek.AddDays(6);
            }
            else
            {
                throw new ArgumentException("Invalid week parameter. Use 'current' or 'previous'.");
            }

            // Calculate the specific date for the given day of the week
            DateTime targetDate = startOfWeek.AddDays(day - 1);

            // Query the database for the total amount of orders on the target date
            decimal totalAmount = _orderRepository.GetListBySpec(new OrderSpecification.GetAllOrders()).Result
                .Where(o => o.OrderDate.Date == targetDate.Date)
                .Sum(o => o.OrderTotal ?? 0);  // Using OrderTotal from OrderDTO

            return totalAmount;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        public async Task<ActionResult<decimal>> GetMonthlyOrderTotal(int month)
        {
            var today = DateTime.UtcNow;
            var startOfMonth = new DateTime(today.Year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var totalAmount = _orderRepository.GetListBySpec(new OrderSpecification.GetAllOrders()).Result
                .Where(order => order.OrderDate >= startOfMonth && order.OrderDate <= endOfMonth)
                .Sum(order => order.OrderTotal ?? 0);

            return totalAmount;
        }

        public async Task<ActionResult<DailyOrderTotalDTO>> GetOrderTotalForSpecificDay(int day)
        {
            var today = DateTime.UtcNow;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            if (day < 1 || day > DateTime.DaysInMonth(today.Year, today.Month))
            {
                return null;
            }

            var targetDate = startOfMonth.AddDays(day - 1); // Calculate the specific date

            // Fetch orders for the specific day
            var dailyTotal = _orderRepository.GetListBySpec(new OrderSpecification.GetAllOrders()).Result
                .Where(order => order.OrderDate.Date == targetDate.Date)
                .GroupBy(order => order.OrderDate.Date)
                .Select(group => new DailyOrderTotalDTO
                {
                    Date = group.Key,
                    TotalOrders = group.Count(),
                    TotalAmount = group.Sum(order => order.OrderTotal ?? 0)
                })
                .FirstOrDefault();

            if (dailyTotal == null)
            {
                // If no orders found for the day, return 0 total
                dailyTotal = new DailyOrderTotalDTO
                {
                    Date = targetDate.Date,
                    TotalOrders = 0,
                    TotalAmount = 0
                };
            }

            return dailyTotal;
        }



        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var result = await _orderRepository.GetListBySpec(new OrderSpecification.GetAllOrders());
            return _mapper.Map<List<OrderDTO>>(result);
        }
        public async Task<List<OrderDTO>> GetOrderByIdAsync(string Id, int page)
        {
            var result = await _orderRepository.GetListBySpec(new GetOrderById(Id, page));
            return _mapper.Map<List<OrderDTO>>(result);
        }
        public async Task<int> GetCountOrderByIdAsync(string Id)
        {
            var orders = await _orderRepository.GetListBySpec(new GetOrderCountByIdSpecification(Id));
            return orders.Count();
        }
        private async Task<AddressEntity> CreateAddressAsync(OrderCreateDTO orderCreateDTO)
        {
            AddressEntity address = new AddressEntity()
            {
                Country = orderCreateDTO.Country,
                City = orderCreateDTO.City,
                State = orderCreateDTO.State,
                Street = orderCreateDTO.Street,
            };
            await _addressRepository.InsertAsync(address);
            await _addressRepository.SaveAsync();
            return address;
        }
        private async Task<OrderPayment> CreateOrderPaymentAsync(OrderCreateDTO orderCreateDTO)
        {
            OrderPayment orderPayment = new OrderPayment()
            {
                Payment = orderCreateDTO.Payment,
                PaymentMethod = orderCreateDTO.PaymentMethod,
                CardHolderName = orderCreateDTO.CardHolderName,
                CardNumber = orderCreateDTO.CardNumber,
                CardMonthExpires = orderCreateDTO.CardMonthExpires,
                CardYearExpires = orderCreateDTO.CardYearExpires,
            };

            await _orderPaymentRepository.InsertAsync(orderPayment);
            await _orderPaymentRepository.SaveAsync();
            return orderPayment;
        }
        private (decimal total, decimal tax, decimal subtotal, decimal accrued) CalculateOrderTotals(IEnumerable<BagItems> bagItems, decimal discount)
        {
            decimal total = bagItems.Sum(p => p.Quantity * p.Product.Price);
            total = total > discount ? total - discount : 0;
            decimal tax = (bagItems.Sum(p => p.Quantity * p.Product.Price) / (100 + 20)) * 20;
            decimal subtotal = total - tax;
            decimal accrued = (0.50m / 100) * total;
            return (total, tax, subtotal, accrued);
        }
        private async Task<Order> CreateOrderAsync(OrderCreateDTO orderCreateDTO, int addressId, string userId, decimal total, decimal tax, decimal subtotal)
        {
            Order order = new Order()
            {
                AddressId = addressId,
                Email = orderCreateDTO.Email,
                EmailUser = orderCreateDTO.EmailUser,
                FirstName = orderCreateDTO.FirstName,
                LastName = orderCreateDTO.LastName,
                OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                PhoneNumber = orderCreateDTO.PhoneNumber,
                UserId = userId,
                OrderTotal = total,
                Tax = tax,
                Subtotal = subtotal,
                Discount = orderCreateDTO.Discount,
            };
            await _orderRepository.InsertAsync(order);
            await _orderRepository.SaveAsync();
            return order;
        }
        private async Task UpdateUserAsync(User user, decimal discount, decimal accrued)
        {
            user.BagId = null;
            user.BonusBalance -= discount;
            user.BonusBalance += accrued;
            await _userManager.UpdateAsync(user);
        }
        private async Task CreateOrderItemsAsync(IEnumerable<BagItems> bagItems, int orderId, OrderCreateDTO orderCreateDTO)
        {
            foreach (var item in bagItems)
            {
                var orderPayment = await CreateOrderPaymentAsync(orderCreateDTO);
                OrderItems orderItems = new OrderItems()
                {
                    Quantity = item.Quantity,
                    Name_en = item.Product.Name_en,
                    Name_uk = item.Product.Name_uk,
                    Name_es = item.Product.Name_es,
                    Name_fr = item.Product.Name_fr,
                    Description_en = item.Product.Description_en,
                    Description_uk = item.Product.Description_uk,
                    Description_es = item.Product.Description_es,
                    Description_fr = item.Product.Description_fr,
                    Price = item.Product.Price,
                    Size = item.Size,
                    Article = item.Product.Article,
                    ImagePath = item.Product.Images.OrderBy(img => img.Id).FirstOrDefault()?.ImagePath,
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    Step = 0,
                    Status = "Order placed",
                    OrderPaymentId = orderPayment.Id,
                };
                await _orderItemsRepository.InsertAsync(orderItems);
                await _orderItemsRepository.SaveAsync();
            }
        }
        private async Task ClearBagAsync(int bagId)
        {
            await _bagRepository.DeleteAsync(bagId);
            await _bagRepository.SaveAsync();
        }
        private async Task CreateUserBonusesAsync(string userId, int orderId, decimal discount, decimal accrued)
        {
            if (discount != 0)
            {

                UserBonuses bonuses_Redeemed = new UserBonuses()
                {
                    OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    UserId = userId,
                    Name = orderId,
                    BonusesAccrued = discount,
                    BonusesDescription_en = "Purchase of goods",
                    BonusesDescription_uk = "Купівля товару",
                    BonusesDescription_es = "Compra de bienes",
                    BonusesDescription_fr = "Achat de biens",
                    BonusesOperation_en = "Redeemed",
                    BonusesOperation_uk = "Списання",
                    BonusesOperation_es = "Redimida",
                    BonusesOperation_fr = "Rachetée",
                };
                await _userBonuses.InsertAsync(bonuses_Redeemed);
                await _userBonuses.SaveAsync();
            }

            UserBonuses bonuses_Accrual = new UserBonuses()
            {
                OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                UserId = userId,
                Name = orderId,
                BonusesAccrued = accrued,
                BonusesDescription_en = "Purchase bonus",
                BonusesDescription_uk = "Бонус за покупку",
                BonusesDescription_es = "Bono de compra",
                BonusesDescription_fr = "Bonus d'achat",
                BonusesOperation_en = "Accrual",
                BonusesOperation_uk = "Нарахування",
                BonusesOperation_es = "Devengo",
                BonusesOperation_fr = "Accumulation",
            };
            await _userBonuses.InsertAsync(bonuses_Accrual);
            await _userBonuses.SaveAsync();
        }

        public async Task<List<OrderItemsDTO>> OrderByPageAsync(int page, int pageSize, int[] step)
        {
            var orderItems = await _orderItemsRepository.GetListBySpec(new OrderItemsSpecification.GetOrderItemsSpecificationByStep(page, pageSize, step));
            return _mapper.Map<List<OrderItemsDTO>>(orderItems);
        }

        public async Task<int> OrderQuantityAsync(int[] step)
        {
            var orderItems = await _orderItemsRepository.GetListBySpec(new OrderItemsSpecification.GetOrderQuantitySpecificationByStep(step));
            return orderItems.Count();
        }

        public async Task EditOrderItemsAsync(OrderItemsDTO orderItemsDTO)
        {
            OrderItems orderItems = await _orderItemsRepository.GetByIDAsync(orderItemsDTO.Id);

            if (orderItemsDTO.Status != null)
            {
                var IsStock = await _storageService.GetStorageSizeByProductIdAsync(orderItems.ProductId, orderItems.Size);
                switch (orderItemsDTO.Status)
                {
                    case "Order placed":
                        orderItems.Step = 0;
                        break;
                    case "Processing":
                        if (orderItems.Step < 1 && IsStock.ProductQuantity >= orderItems.Quantity)
                        {
                            await _storageService.ChangeDecreaseQuantityStorageAsync(orderItemsDTO.ProductId, orderItemsDTO.Size, orderItemsDTO.Quantity);
                            orderItems.Step = 1;
                            orderItems.Status = orderItemsDTO.Status;
                            orderItems.DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        }
                        break;
                    case "Shipped":
                        if (orderItems.Step < 2)
                        {
                            if (orderItems.Step < 1)
                            {
                                await _storageService.ChangeDecreaseQuantityStorageAsync(orderItemsDTO.ProductId, orderItemsDTO.Size, orderItemsDTO.Quantity);
                            }
                            orderItems.Step = 2;
                            orderItems.Status = orderItemsDTO.Status;
                            orderItems.DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        }
                        break;
                    case "Delivered":
                        if (orderItems.Step < 3)
                        {
                            if (orderItems.Step < 1)
                            {
                                await _storageService.ChangeDecreaseQuantityStorageAsync(orderItemsDTO.ProductId, orderItemsDTO.Size, orderItemsDTO.Quantity);
                            }
                            orderItems.Step = 3;
                            orderItems.Status = orderItemsDTO.Status;
                            orderItems.DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        }
                        break;
                    case "Cancelled":
                        if (orderItems.Step >= 1 && orderItems.Step <= 4)
                        {
                            await _storageService.ChangeIncreaseQuantityStorageAsync(orderItemsDTO.ProductId, orderItemsDTO.Size, orderItemsDTO.Quantity);
                        }
                        orderItems.Step = 4;
                        orderItems.Status = orderItemsDTO.Status;
                        orderItems.DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        var paymentCancelled = await _orderPaymentRepository.GetByIDAsync(orderItems.Id);
                        paymentCancelled.Payment = "The money was returned";
                        await _orderPaymentRepository.UpdateAsync(paymentCancelled);
                        await _orderPaymentRepository.SaveAsync();
                        break;
                    case "Returned":
                        if (orderItems.Step >= 1 && orderItems.Step <= 4)
                        {
                            orderItems.Step = 5;
                            orderItems.Status = orderItemsDTO.Status;
                            orderItems.DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            await _storageService.ChangeIncreaseQuantityStorageAsync(orderItemsDTO.ProductId, orderItemsDTO.Size, orderItemsDTO.Quantity);
                            var paymentReturned = await _orderPaymentRepository.GetByIDAsync(orderItems.Id);
                            paymentReturned.Payment = "The money was returned";
                            await _orderPaymentRepository.UpdateAsync(paymentReturned);
                            await _orderPaymentRepository.SaveAsync();
                        }
                        break;
                    default:
                        break;
                }
                try
                {
                    await _orderItemsRepository.UpdateAsync(orderItems);
                    await _orderItemsRepository.SaveAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw new Exception();
                }
            }
        }
    }
}
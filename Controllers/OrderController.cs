
using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce.Dtos;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<string>>> GetOrderCountries()
        {
            var countries = await Task.Run(() =>
                _context.Orders
                    .Where(o => !string.IsNullOrEmpty(o.Location))
                    .AsEnumerable()
                    .Select(o => o.Location.Split(',')[0].Trim())
                    .Distinct()
                    .ToList()
            );

            return Ok(countries);
        }

        [HttpGet("countries/top")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopCountries()
        {
            var orders = await _context.Orders
                .Where(o => !string.IsNullOrEmpty(o.Location))
                .ToListAsync(); 

            var topCountries = orders
                .Select(o => o.Location.Split(',')[0].Trim())
                .GroupBy(country => country)
                .Select(g => new {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .ToList();

            return Ok(topCountries);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(int userId)
        {
            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .ToListAsync();

            if (!userOrders.Any())
            {
                return NotFound($"No orders found for user with ID {userId}.");
            }

            return Ok(userOrders);
        }

        [HttpGet("popular-products")]
        public async Task<ActionResult<IEnumerable<object>>> GetPopularProducts(int limit = 20)
        {
            var popularProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Product = g.First().Product,
                    OrderCount = g.Count(),
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    LastOrderDate = g.Max(oi => oi.Order.OrderDate)
                })
                .OrderByDescending(p => p.TotalQuantity) 
                .Take(limit)
                .ToListAsync();

            return Ok(popularProducts);
        }

        [HttpGet("product/{productId}/count")]
        public async Task<ActionResult<int>> GetOrderCountForProduct(int productId)
        {
            var orderCount = await _context.OrderItems
                .Where(oi => oi.ProductId == productId)
                .CountAsync();

            return Ok(orderCount);
        }

        [HttpPost("statistics")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderStatistics([FromBody] ProductIdsDto request)
        {
            if (request?.ProductIds == null || !request.ProductIds.Any())
            {
                return BadRequest("Product IDs are required.");
            }

            var statistics = await _context.OrderItems
                .Where(oi => request.ProductIds.Contains(oi.ProductId))
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalOrders = g.Count(),
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    LastOrderDate = g.Max(oi => oi.Order.OrderDate)
                })
                .ToListAsync();

            return Ok(statistics);
        }

        [HttpGet("all-statistics")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllOrderStatistics()
        {
            var allStatistics = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalOrders = g.Count(),
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    LastOrderDate = g.Max(oi => oi.Order.OrderDate),
                    AverageQuantityPerOrder = g.Average(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.TotalOrders)
                .ToListAsync();

            return Ok(allStatistics);
        }

        [HttpGet("trending")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrendingProducts(int days = 30, int limit = 10)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            var trendingProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.OrderDate >= cutoffDate)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Product = g.First().Product,
                    RecentOrderCount = g.Count(),
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    LastOrderDate = g.Max(oi => oi.Order.OrderDate)
                })
                .OrderByDescending(p => p.RecentOrderCount)
                .Take(limit)
                .ToListAsync();

            return Ok(trendingProducts);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange(DateTime start, DateTime end)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.Equals(dto.DeliveryType, "nominated", StringComparison.OrdinalIgnoreCase) && !dto.NominatedDate.HasValue)
            {
                ModelState.AddModelError(nameof(dto.NominatedDate), "Nominated date is required for nominated delivery type.");
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = dto.TotalPrice,
                TotalAmount = dto.TotalAmount,
                DeliveryType = dto.DeliveryType,
                Email = dto.Email,
                Location = dto.Location,
                OrderItems = new List<OrderItem>()
            };

            switch (order.DeliveryType?.ToLowerInvariant())
            {
                case "standard":
                    order.DeliveryDate = order.OrderDate.AddDays(7).Date;
                    break;
                case "flash":
                    order.DeliveryDate = order.OrderDate.AddDays(1).Date;
                    break;
                case "nominated":
                    if (dto.NominatedDate.HasValue)
                    {
                        if (dto.NominatedDate.Value.Date < order.OrderDate.Date)
                        {
                            ModelState.AddModelError(nameof(dto.NominatedDate), "Nominated delivery date cannot be in the past.");
                            return BadRequest(ModelState);
                        }
                        order.DeliveryDate = dto.NominatedDate.Value.Date;
                    }
                    break;
                default:
                    order.DeliveryDate = null;
                    break;
            }

            foreach (var itemDto in dto.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}/delivery-type")]
        public async Task<IActionResult> UpdateDeliveryType(int id, [FromBody] string deliveryType)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.DeliveryType = deliveryType;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    
    public class ProductIdsDto
    {
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
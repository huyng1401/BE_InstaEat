using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<OrderViewModel>>> GetOrdersByDateRange([FromQuery] DateTime minDate, [FromQuery] DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(minDate, maxDate, pageIndex, pageSize);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderViewModel>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }

        [HttpPost("add-points")]
        public async Task<IActionResult> AddPointsToRestaurantWallet([FromQuery] int restaurantId, [FromQuery] int packageId)
        {
            try
            {
                var isSuccess = await _orderService.AddPointsToRestaurantWalletAsync(restaurantId, packageId);
                if (!isSuccess)
                {
                    return BadRequest("Unable to add points.");
                }

                return Ok("Points added to restaurant wallet successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderViewModel updateOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isUpdated = await _orderService.UpdateOrderAsync(id, updateOrderViewModel);
                if (!isUpdated)
                {
                    return NotFound("Order not found.");
                }

                return Ok("Successfully Updated!!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var isDeleted = await _orderService.DeleteOrderAsync(id);
            if (!isDeleted)
            {
                return NotFound("Order not found.");
            }

            return Ok("Successfully Deleted!!");
        }
    }
}

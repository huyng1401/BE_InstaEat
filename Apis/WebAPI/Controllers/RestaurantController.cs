using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.RestaurantViewModels;
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
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<RestaurantViewModel>>> GetRestaurants([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var restaurants = await _restaurantService.GetRestaurantsAsync(pageIndex, pageSize);
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantViewModel>> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }
            return Ok(restaurant);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] UpdateRestaurantViewModel updateRestaurantViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = await _restaurantService.UpdateRestaurantAsync(id, updateRestaurantViewModel);
            if (!isUpdated)
            {
                return NotFound("Restaurant not found.");
            }

            return Ok("Successfully Updated!!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var isDeleted = await _restaurantService.DeleteRestaurantAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok("Successfully Deleted!!");
        }
    }
}

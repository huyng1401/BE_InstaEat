using Application.Interfaces;
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
        public async Task<ActionResult<List<RestaurantViewModel>>> GetRestaurants()
        {
            var restaurants = await _restaurantService.GetRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantViewModel>> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantViewModel>> CreateRestaurant([FromBody] CreateRestaurantViewModel createRestaurantViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = await _restaurantService.CreateRestaurantAsync(createRestaurantViewModel);
            if (restaurant == null)
            {
                return BadRequest("Unable to create restaurant.");
            }

            return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.RestaurantId }, restaurant);
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
                return NotFound();
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

using Application.Interfaces;
using Application.ViewModels.ReviewViewModels;
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
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewViewModel>>> GetReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewViewModel>> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review not found.");
            }
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewViewModel>> CreateReview([FromBody] CreateReviewViewModel createReviewViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = await _reviewService.CreateReviewAsync(createReviewViewModel);
            if (review == null)
            {
                return BadRequest("Unable to create review.");
            }

            return CreatedAtAction(nameof(GetReviewById), new { id = review.ReviewId }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewViewModel updateReviewViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isUpdated = await _reviewService.UpdateReviewAsync(id, updateReviewViewModel);
                if (!isUpdated)
                {
                    return NotFound("Review not found.");
                }

                return Ok("Successfully Updated!!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var isDeleted = await _reviewService.DeleteReviewAsync(id);
            if (!isDeleted)
            {
                return NotFound("Review not found.");
            }

            return Ok("Successfully Deleted!!");
        }
    }
}

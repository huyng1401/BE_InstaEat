using Application.Commons;
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

        [HttpGet("restaurant")]
        public async Task<ActionResult<Pagination<ReviewViewModel>>> GetReviewsByStatus(int status, int pageIndex = 0, int pageSize = 10)
        {
            var reviews = await _reviewService.GetReviewsByStatusAsync(status, pageIndex, pageSize);
            return Ok(reviews);
        }


        [HttpGet("restaurant/{id}")]
        public async Task<ActionResult<ReviewViewModel>> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review not found.");
            }
            return Ok(review);
        }

        [HttpPost("restaurant")]
        public async Task<ActionResult<ReviewViewModel>> CreateReview([FromForm] CreateReviewViewModel createReviewViewModel)
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
        [HttpPost("accept/{reviewId}")]
        public async Task<IActionResult> AcceptReview(int reviewId)
        {
            try
            {
                var isAccepted = await _reviewService.AcceptReviewAsync(reviewId);
                if (!isAccepted)
                {
                    return BadRequest("Unable to accept review.");
                }
                return Ok("Review accepted and points transferred.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("restaurant/{id}")]
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

        [HttpDelete("restaurant/{id}")]
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

using Application.ViewModels.ReviewViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetReviewsAsync();
        Task<ReviewViewModel?> GetReviewByIdAsync(int reviewId);
        Task<ReviewViewModel?> CreateReviewAsync(CreateReviewViewModel review);
        Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewViewModel review);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}

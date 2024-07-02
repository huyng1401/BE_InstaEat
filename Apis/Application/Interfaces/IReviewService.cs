using Application.Commons;
using Application.ViewModels.ReviewViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetReviewsAsync();
        Task<ReviewViewModel?> GetReviewByIdAsync(int reviewId);
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task<ReviewViewModel?> CreateReviewAsync(CreateReviewViewModel review);
        Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewViewModel review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<Pagination<ReviewViewModel>> GetReviewsByStatusAsync(int status, int pageIndex = 0, int pageSize = 10);
        Task<bool> AcceptReviewAsync(int reviewId);
    }
}

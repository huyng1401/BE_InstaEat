using Application.Interfaces;
using Application.ViewModels.ReviewViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ReviewViewModel>> GetReviewsAsync()
        {
            var reviews = await _unitOfWork.ReviewRepository.GetAllNotDeletedAsync();
            return _mapper.Map<List<ReviewViewModel>>(reviews);
        }

        public async Task<ReviewViewModel?> GetReviewByIdAsync(int reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (review != null && review.IsDeleted == false)
            {
                return _mapper.Map<ReviewViewModel>(review);
            }
            return null;
        }

        public async Task<ReviewViewModel?> CreateReviewAsync(CreateReviewViewModel review)
        {
            var reviewEntity = _mapper.Map<Review>(review);
            reviewEntity.Status = 0; // Mặc định là Processing
            reviewEntity.Created = DateTime.Now;

            await _unitOfWork.ReviewRepository.AddAsync(reviewEntity);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<ReviewViewModel>(reviewEntity);
            }
            return null;
        }

        public async Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewViewModel review)
        {
            var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (existingReview == null || existingReview.IsDeleted == true)
            {
                return false;
            }

            // Kiểm tra giá trị của Status
            if (review.Status.HasValue && review.Status != 0 && review.Status != -1 && review.Status != 1)
            {
                throw new ArgumentException("Invalid status value. Status must be 0 (Processing), -1 (Reject), or 1 (Accepted).");
            }

            _mapper.Map(review, existingReview);
            _unitOfWork.ReviewRepository.Update(existingReview);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (review == null || review.IsDeleted == true)
            {
                return false;
            }

            review.IsDeleted = true;
            _unitOfWork.ReviewRepository.Update(review);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}

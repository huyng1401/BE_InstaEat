using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.ReviewViewModels;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient _blobServiceClient;
        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, BlobServiceClient blobServiceClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobServiceClient = blobServiceClient;
        }
        public async Task<Pagination<ReviewViewModel>> GetReviewsByStatusAsync(int status, int pageIndex = 0, int pageSize = 10)
        {
            var reviewsPagination = await _unitOfWork.ReviewRepository.GetReviewsByStatusAsync(status, pageIndex, pageSize);
            var reviewsViewModel = _mapper.Map<Pagination<ReviewViewModel>>(reviewsPagination);
            return reviewsViewModel;
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
        public BlobContainerClient GetBlobContainerClient()
        {
            return _blobServiceClient.GetBlobContainerClient("instaeatcontainer");
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var blobContainerClient = GetBlobContainerClient();
            await blobContainerClient.CreateIfNotExistsAsync();
            var blobClient = blobContainerClient.GetBlobClient(imageFile.FileName);

            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, httpHeaders);
            }

            return blobClient.Uri.AbsoluteUri;
        }
        public async Task<ReviewViewModel?> CreateReviewAsync(CreateReviewViewModel review)
        {
            var customer = await _unitOfWork.AccountRepository.GetByIdAsync(review.CustomerId.Value);
            if (customer == null || customer.IsDeleted == true)
            {
                throw new ArgumentException("Invalid CustomerId.");
            }

            var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(review.RestaurantId.Value);
            if (restaurant == null || restaurant.IsDeleted == true)
            {
                throw new ArgumentException("Invalid RestaurantId.");
            }

            if (customer.RoleId != 2)
            {
                throw new InvalidOperationException("Only customers can create reviews.");
            }

            if (customer.UserId == restaurant.UserId)
            {
                throw new InvalidOperationException("Restaurant cannot review itself.");
            }
            string imageUrl = null;
            if (review.Image != null)
            {
                imageUrl = await UploadImageAsync(review.Image);
            }
            var reviewEntity = _mapper.Map<Review>(review);
            reviewEntity.Image = imageUrl;
            reviewEntity.Status = 0;
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
        public async Task<bool> AcceptReviewAsync(int reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (review == null || review.IsDeleted == true)
            {
                throw new ArgumentException("Review not found or has been deleted.");
            }

            if (review.Status != 0)
            {
                throw new InvalidOperationException("Review is not in a pending state.");
            }
            var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(review.RestaurantId.Value);
            if (restaurant == null || restaurant.IsDeleted == true)
            {
                throw new InvalidOperationException("Restaurant not found or has been deleted.");
            }
            var restaurantUser = await _unitOfWork.AccountRepository.GetByIdAsync(restaurant.UserId.Value);
            if (restaurantUser == null || restaurantUser.IsDeleted == true || restaurantUser.RoleId != 3)
            {
                throw new InvalidOperationException("Restaurant user not found, has been deleted, or is not a restaurant.");
            }
            var restaurantWallet = await _unitOfWork.WalletRepository.GetByIdAsync(restaurant.UserId.Value);
            if (restaurantWallet == null || restaurantWallet.IsDeleted == true)
            {
                throw new InvalidOperationException("Restaurant wallet not found.");
            }
            var user = await _unitOfWork.AccountRepository.GetByIdAsync(review.UserId.Value);
            if (user == null || user.IsDeleted == true || user.RoleId != 2)
            {
                throw new ArgumentException("User not found, has been deleted, or is not a customer.");
            }

            var userWallet = await _unitOfWork.WalletRepository.GetByIdAsync(review.UserId.Value);
            if (userWallet == null)
            {
                throw new InvalidOperationException("User wallet not found.");
            }

            if (restaurantWallet.TotalPoint < 5)
            {
                throw new InvalidOperationException("Restaurant does not have enough points.");
            }

            var transactions = await _unitOfWork.TransactionRepository.GetAllNotDeletedAsync();
            var transactionsToday = transactions
                .Where(t => t.RestaurantId == review.RestaurantId && t.UserId == review.UserId && t.Created.Value.Date == DateTime.UtcNow.Date)
                .Sum(t => t.Amount);

            if (transactionsToday >= 15)
            {
                throw new InvalidOperationException("Daily transfer limit reached for this user.");
            }

            review.Status = 1;
            _unitOfWork.ReviewRepository.Update(review);

            restaurantWallet.TotalPoint -= 5;
            userWallet.TotalPoint += 5;

            _unitOfWork.WalletRepository.Update(restaurantWallet);
            _unitOfWork.WalletRepository.Update(userWallet);

            var transaction = new Transaction
            {
                RestaurantId = review.RestaurantId,
                UserId = review.UserId,
                ReviewId = reviewId,
                Amount = 5,
                Created = DateTime.Now
            };
            await _unitOfWork.TransactionRepository.AddAsync(transaction);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}

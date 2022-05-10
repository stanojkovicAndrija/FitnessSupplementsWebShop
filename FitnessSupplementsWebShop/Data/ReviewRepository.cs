using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public ReviewRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public ReviewEntity CreateReview(ReviewEntity review)
        {
            context.Review.Add(review);
            return review;
        }

        public void DeleteReview(int reviewID)
        {
            context.Review.Remove(context.Review.FirstOrDefault(r => r.ReviewID == reviewID));
        }

        public ReviewEntity GetReviewByID(int reviewID)
        {
            return context.Review.FirstOrDefault(r => r.ReviewID == reviewID);
        }

        public List<ReviewEntity> GetReview()
        {
            return (from r in context.Review select r).ToList();
        }

        public void UpdateReview(ReviewEntity review)
        {
            throw new NotImplementedException();
        }
    }
}
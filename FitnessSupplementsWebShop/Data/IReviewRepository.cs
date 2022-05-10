using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IReviewRepository
    {
        List<ReviewEntity> GetReview();

        ReviewEntity GetReviewByID(int reviewID);

        ReviewEntity CreateReview(ReviewEntity review);

        void UpdateReview(ReviewEntity review);

        void DeleteReview(int reviewID);

        bool SaveChanges();

    }
}

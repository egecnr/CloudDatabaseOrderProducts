using System;

namespace ExtraFunction.DTO
{
    public class CreateReviewDTO
    {
        public Guid ProductId { get; set; }
        public string UserReview { get; set; }
    }
}

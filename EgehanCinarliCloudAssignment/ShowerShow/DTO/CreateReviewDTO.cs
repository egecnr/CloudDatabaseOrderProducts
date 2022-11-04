using System;

namespace UserAndOrdersFunction.DTO
{
    public class CreateReviewDTO
    {
        public Guid ProductId { get; set; }
        public string UserReview { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Model
{
    public class Review
    {
        //Since its anonymous, 
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string UserReview { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}

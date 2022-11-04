using System.Collections.Generic;
using System;
using ShowerShow.Model;

namespace ShowerShow.DTO
{
    public class CreateOrderDTO
    {
        public Dictionary<string, int> allOrderItems { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool IsOrderSent { get; set; } = false;
        public DateTime? DateOfShipment { get; set; } = null;
    }
}

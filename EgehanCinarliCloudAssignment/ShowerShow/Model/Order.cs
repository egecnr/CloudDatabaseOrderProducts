using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShowerShow.Model
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public Dictionary<string, int> allOrderItems { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool IsOrderSent { get; set; } = false;
        public DateTime? DateOfShipment { get; set; } = null;
    }
}

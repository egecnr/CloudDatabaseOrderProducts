using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Model
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public List<string> ProductIds { get; set; } = new List<string>();
        public Guid UserId { get; set; }
        public DateTime DateOfOrder { get; set; } = DateTime.Now; //Initial creation of an order always has the creation date.
        public bool IsOrderSent { get; set; } = false;
        public DateTime? DateOfShipment { get; set; } = null;
        public string Remarks { get; set; }
    }
}

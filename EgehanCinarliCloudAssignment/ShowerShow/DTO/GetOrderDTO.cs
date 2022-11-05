using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAndOrdersFunction.Model;

namespace UserAndOrdersFunction.DTO
{
    public class GetOrderDTO
    {
        public List<Product> Products { get; set; } //Just for better readability
        public DateTime DateOfOrder { get; set; }
        public bool IsOrderSent { get; set; } = false;
        public DateTime? DateOfShipment { get; set; } = null;
        public string Remarks { get; set; }
    }
}

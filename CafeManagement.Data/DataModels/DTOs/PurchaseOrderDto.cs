using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.DTOs
{
    public class PurchaseOrderDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public string? Comments { get; set; }
    }
}

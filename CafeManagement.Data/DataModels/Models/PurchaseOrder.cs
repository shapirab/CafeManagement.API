using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public string? Comments { get; set; }
    }
}

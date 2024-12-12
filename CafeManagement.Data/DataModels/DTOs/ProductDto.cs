using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}

using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<(IEnumerable<ProductEntity>, PaginationMetaData)> GetAllProductsAsync
            (int? categoryId, string? searchQuery, int pageNumber, int pageSize);
        Task<ProductEntity?> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductEntity product);
        Task DeleteProductAsync(int id);
        Task<bool> IsProductExists(int id);
        Task<bool> SaveChangesAsync();
    }
}

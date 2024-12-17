using CafeManagement.Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrderEntity>> GetAllPurchaseOrdersAsync();
        Task<(IEnumerable<PurchaseOrderEntity>, PaginationMetaData)> GetAllPurchaseOrdersAsync
            (int? productId, int? userId, string? searchQuery, int pageNumber, int pageSize);
        Task<PurchaseOrderEntity?> GetPurchaseOrderByIdAsync(int id);
        Task AddPurchaseOrderAsync(PurchaseOrderEntity purchaseOrder);
        Task DeletePurchaseOrderAsync(int id);
        Task<bool> IsPurchaseOrderExists(int id);
        Task<bool> SaveChangesAsync();
    }
}

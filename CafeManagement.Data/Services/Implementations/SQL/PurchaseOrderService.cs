using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DbContexts;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Implementations.SQL
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly CafeManagementDbContext db;

        public PurchaseOrderService(CafeManagementDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddPurchaseOrderAsync(PurchaseOrderEntity purchaseOrder)
        {
            await db.PurchaseOrders.AddAsync(purchaseOrder);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            PurchaseOrderEntity? purchaseOrder = await GetPurchaseOrderByIdAsync(id);
            if(purchaseOrder != null)
            {
                db.PurchaseOrders.Remove(purchaseOrder);
            }
        }

        public async Task<IEnumerable<PurchaseOrderEntity>> GetAllPurchaseOrdersAsync()
        {
            return await db.PurchaseOrders.OrderBy(po => po.Name).ToListAsync();
        }

        public async Task<(IEnumerable<PurchaseOrderEntity>, PaginationMetaData)> GetAllPurchaseOrdersAsync
            (int? productId, int? userId, string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<PurchaseOrderEntity> collection = db.PurchaseOrders as IQueryable<PurchaseOrderEntity>;

            if(productId != null)
            {
                collection = collection.Where(po => po.ProductId == productId);
            }
            if(userId != null)
            {
                collection = collection.Where(po => po.UserId == userId);
            }
            if(searchQuery != null)
            {
                collection = collection.Where(po => po.Name.Contains(searchQuery));
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(prod => prod.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collection, paginationMetadata);
        }

        public async Task<PurchaseOrderEntity?> GetPurchaseOrderByIdAsync(int id)
        {
            return await db.PurchaseOrders.Where(po => po.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsPurchaseOrderExists(int id)
        {
            PurchaseOrderEntity? purchaseOrder = await GetPurchaseOrderByIdAsync(id);
            return purchaseOrder != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}

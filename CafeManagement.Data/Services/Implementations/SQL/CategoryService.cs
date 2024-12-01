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
    public class CategoryService : ICategoryService
    {
        private readonly CafeManagementDbContext db;

        public CategoryService(CafeManagementDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync()
        {
            return await db.Categories.ToListAsync();
        }

        public async Task<(IEnumerable<CategoryEntity>, PaginationMetaData)> GetAllCategoriesAsync
            (string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<CategoryEntity> collection = db.Categories as IQueryable<CategoryEntity>;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(category => category.Name.Contains(searchQuery));
            }

            int totalItemCount = await collection.CountAsync();
            PaginationMetaData paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(category => category.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
        {
            return await db.Categories.FindAsync(id);
        }

        public async Task AddCategoryAsync(CategoryEntity category)
        {
            await db.Categories.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            CategoryEntity? categoryEntity = await GetCategoryByIdAsync(id);
            if(categoryEntity != null)
            {
                db.Categories.Remove(categoryEntity);
            }
        }

        public async Task<bool> IsCategoryExists(int id)
        {
            CategoryEntity? categoryEntity = await GetCategoryByIdAsync(id);
            return categoryEntity != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}

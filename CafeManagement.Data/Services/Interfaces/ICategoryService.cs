using CafeManagement.Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync();
        Task<(IEnumerable<CategoryEntity>, PaginationMetaData)> GetAllCategoriesAsync
            (string? searchQuery, int pageNumber, int pageSize);
        Task<CategoryEntity?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryEntity category);
        Task DeleteCategoryAsync(int id);
        Task<bool> IsCategoryExists(int id);
        Task<bool> SaveChangesAsync();
    }
}

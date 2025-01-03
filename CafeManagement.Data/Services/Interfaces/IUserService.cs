using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task<(IEnumerable<UserEntity>, PaginationMetaData)> GetAllUsersAsync
            (Role? role, string? searchQuery, int pageNumber, int pageSize);
        Task<UserEntity?> GetUserByIdAsync (int id);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task AddUserAsync (UserEntity user);
        Task DeleteUserAsync (int id);
        Task<bool> IsUserExists(int id);
        Task<bool> IsUserExists(string username);
        Task<bool> SaveChangesAsync();
    }
}
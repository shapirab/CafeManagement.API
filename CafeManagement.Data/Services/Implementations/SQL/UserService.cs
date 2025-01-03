using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
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
    public class UserService : IUserService
    {
        private readonly CafeManagementDbContext db;

        public UserService(CafeManagementDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddUserAsync(UserEntity user)
        {
            await db.Users.AddAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            UserEntity? userEntity = await GetUserByIdAsync(id);
            if(userEntity != null)
            {
                db.Users.Remove(userEntity);
            }
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await db.Users.OrderBy(user => user.FullName).ToListAsync();
        }

        public async Task<(IEnumerable<UserEntity>, PaginationMetaData)> GetAllUsersAsync
            (Role? role, string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<UserEntity> collection = db.Users as IQueryable<UserEntity>;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(user => user.FirstName.Contains(searchQuery) ||
                    user.LastName.Contains(searchQuery) || user.Email.Contains(searchQuery));
            }
            if (role != null)
            {
                collection = collection.Where(user => user.UserRole == role);
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(user => user.LastName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await db.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await db.Users.Where(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserExists(int id)
        {
            return await GetUserByIdAsync(id) != null;
        }

        public async Task<bool> IsUserExists(string username)
        {
            return await GetUserByUsernameAsync(username) != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() >= 0;
        }
    }
}

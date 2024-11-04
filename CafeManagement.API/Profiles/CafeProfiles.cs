using AutoMapper;
using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;

namespace CafeManagement.API.Profiles
{
    public class CafeProfiles : Profile
    {
        public CafeProfiles()
        {
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();

            CreateMap<Product, ProductEntity>();
            CreateMap<ProductEntity, Product>();

            CreateMap<Category, CategoryEntity>();
            CreateMap<CategoryEntity, Category>();
        }
    }
}

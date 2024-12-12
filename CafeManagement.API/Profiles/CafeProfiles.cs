using AutoMapper;
using CafeManagement.Data.DataModels.DTOs;
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
            CreateMap<UserDto, UserEntity>();
            CreateMap<UserEntity, UserDto>();

            CreateMap<Product, ProductEntity>();
            CreateMap<ProductEntity, Product>();
            CreateMap<ProductDto, ProductEntity>();
            CreateMap<ProductEntity, ProductDto>();

            CreateMap<Category, CategoryEntity>();
            CreateMap<CategoryEntity, Category>();
            CreateMap<CategoryWithoutProductsDto, CategoryEntity>();
            CreateMap<CategoryEntity, CategoryWithoutProductsDto>();
            CreateMap<CategoryDto, CategoryEntity>();
            CreateMap<CategoryEntity, CategoryDto>();

            
        }
    }
}

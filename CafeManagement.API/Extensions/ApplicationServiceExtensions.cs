using CafeManagement.Data.DbContexts;
using CafeManagement.Data.Services.Implementations.SQL;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<CafeManagementDbContext>(
                dbContextOptions => dbContextOptions.UseSqlServer(
                    config.GetConnectionString("ConnectionStrings:CafeManagmentDB")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}

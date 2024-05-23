using LibraryApi.Services;
using LibraryApi.Interfaces;

namespace LibraryApi.Extentions{
 public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<RedisService>(); 
        }
    }
}
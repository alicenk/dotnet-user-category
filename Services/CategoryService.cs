using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LibraryDbContext _context;
        private readonly RedisService _redisService;

        public CategoryService(LibraryDbContext context, RedisService redisService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        }

        public List<Category> GetAll() {
            List<Category> category = [.. _context.Category];
            return category;
        }

        public Category GetById(int id) {
            return _context.Category.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Category category)
        {
           // Veritabanına yeni bir kitap ekleyin
            _context.Category.Add(category);
            _context.SaveChanges();

            // Redis'e ekleyin
            _redisService.SetAsync($"category:{category.Id}", category.Name);
        }

        public void Update(int id, Category updatedCategory)
        {
            var existingCategory = _context.Category.Find(id) ?? throw new ArgumentException("Category not found");

            // Kitabın özelliklerini güncelleyin
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

             // Değişiklikleri kaydedin
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            // Silinecek kategori bulun
            var categoryToDelete = _context.Category.Find(id) ?? throw new ArgumentException("Category not found");

            // Kitabı veritabanından silin
            _context.Category.Remove(categoryToDelete);
            _context.SaveChanges();
        }

        public async Task<IActionResult> GetCategoryFromRedis(int id)
        {
            var key = $"category:{id}";
            var value = await _redisService.GetAsync(key) ?? throw new NotImplementedException();
           return new OkObjectResult(value);
        }
    }
}
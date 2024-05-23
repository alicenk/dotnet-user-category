using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
        void Update(int id, Category updatedCategory);
        void Delete(int id);
        Task<IActionResult> GetCategoryFromRedis(int id);
    }
}

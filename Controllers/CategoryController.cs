using LibraryApi.Models;
using LibraryApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/Category")]
    public class CategoryController: ControllerBase
    { 
        private readonly ICategoryService _categoryService;

         public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryService.Add(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpGet]
        public ActionResult<List<Category>> Get() => _categoryService.GetAll();

        [HttpGet("from-cache/{id}")]
        public async Task<IActionResult> GetCategoryFromRedis(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryFromRedis(id);
                return Ok(category);
            }
            catch (Exception)
            {
                // Hata durumunda uygun bir HTTP yanıtı dönülebilir
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly Interface_CategoryRepository _categoryRepository;
        public CategoryController(Interface_CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            var category = await _categoryRepository.GetCategoriesAsync();
            ViewData["CurrentPage"] = "Danh mục";

            return View(category);
        }
        private async Task<string?> SaveImage(IFormFile image)
        {

            var savePath = Path.Combine("wwwroot/categoryIcons", image.FileName); //

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/categoryIcons/" + image.FileName;
        }
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category,IFormFile CategoryIcon)
        {
            if (ModelState.IsValid)
            {
                if(CategoryIcon != null)
                {
                    category.CategoryIcon = await SaveImage(CategoryIcon);
                }
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category,IFormFile CategoryIcon)
        {
            ModelState.Remove("CategoryIcon");
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingCategory = _categoryRepository.GetByIdAsync(id).Result;
                if (CategoryIcon == null)
                {
                    category.CategoryIcon = existingCategory.CategoryIcon;
                }
                existingCategory.Name = category.Name;
                existingCategory.CategoryIcon = category.CategoryIcon;
                await _categoryRepository.UpdateAsync(existingCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

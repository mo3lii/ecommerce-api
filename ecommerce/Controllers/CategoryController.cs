using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		UnitOfWork unit;
		public CategoryController(UnitOfWork unit)
		{
			this.unit = unit;
		}
		[HttpGet]
		public IActionResult GetAll()
		{
			var categories = unit.CategoryRepository.GetAll("Products");
			if (categories.Count == 0) return NotFound();
			var categoriesDTO = new List<CategoryDTO>();
			foreach (var category in categories)
			{
				categoriesDTO.Add(new CategoryDTO()
				{
					Id = category.Id,
					Name = category.Name,
					Description = category.Description,
					ProductsCount = category.Products.Where(p => p.isDeleted == false && p.isActive == true).Count()
				});
			}
			return Ok(categoriesDTO);
		}
		[HttpPost]
		public IActionResult add(CategoryPostDTO category)
		{
			unit.CategoryRepository.Insert(new ProductCategory()
			{
				Name = category.name,
				Description = category.description,
			});
			unit.SaveChanges();
			return Ok(category);
		}
		[HttpDelete("{id}")]
		public IActionResult delete(int id)
		{
            var category = unit.CategoryRepository.GetById(id);
            if (category != null)
            {
                category.isDeleted = true;
                unit.CategoryRepository.Update(category);
                unit.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
		[HttpPut("{id}")]
		public IActionResult update(int id,CategoryPostDTO categoryDTO)
		{
            var category = unit.CategoryRepository.GetById(id);
            if (category == null) return NotFound();
            var updatedCategory = new ProductCategory()
            {
             Id = id,
			 Name = categoryDTO.name,
			 Description=categoryDTO.description,
            };
          
            unit.CategoryRepository.Update(updatedCategory);
            unit.SaveChanges();
            return Ok(updatedCategory);
        }
	}
}

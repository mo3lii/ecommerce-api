using ecommerce.DTO;
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
			var categories = unit.CategoryRepository.GetAll();
			if (categories.Count == 0) return NotFound();
			var categoriesDTO = new List<CategoryDTO>();
			foreach (var category in categories)
			{
				categoriesDTO.Add(new CategoryDTO()
				{
					Id = category.Id,
					Name = category.Name,
					Description = category.Description,
				});
			}
			return Ok(categoriesDTO);
		}
	}
}

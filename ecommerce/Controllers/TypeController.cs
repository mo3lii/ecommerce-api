using ecommerce.DTO;
using ecommerce.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TypeController : ControllerBase
	{
		UnitOfWork unit;
		public TypeController(UnitOfWork unit)
		{
			this.unit = unit;
		}
		[HttpGet]
		public IActionResult GetAll()
		{
			
			var types = unit.TypeRepository.GetAll();
			if (types.Count == 0) return NotFound();
			var typesDTO = new List<TypeDTO>();
			foreach (var type in types)
			{
				typesDTO.Add(new TypeDTO()
				{
					Id = type.Id,
					Name = type.Name,
					Description = type.Description,
				});
			}
			return Ok(typesDTO);
		}
	}
}

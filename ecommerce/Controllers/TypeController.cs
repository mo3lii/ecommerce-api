using ecommerce.DTO;
using ecommerce.Models;
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
            var types = unit.TypeRepository.GetAll("Products");
            if (types.Count == 0) return NotFound();
            var typesDTO = new List<TypeDTO>();
            foreach (var type in types)
            {
                typesDTO.Add(new TypeDTO()
                {
                    Id = type.Id,
                    Name = type.Name,
                    Description = type.Description,
                    ProductsCount = type.Products.Where(p => p.isDeleted == false && p.isActive == true).Count()
                });
            }
            return Ok(typesDTO);
        }
        [HttpPost]
        public IActionResult add(TypePostDTO type)
        {
            unit.TypeRepository.Insert(new ProductType()
            {
                Name = type.name,
                Description = type.description,
            });
            unit.SaveChanges();
            return Ok(type);
        }
        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var type = unit.TypeRepository.GetById(id);
            if (type != null)
            {
                type.isDeleted = true;
                unit.TypeRepository.Update(type);
                unit.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public IActionResult update(int id, TypePostDTO typeDTO)
        {
            var type = unit.TypeRepository.GetById(id);
            if (type == null) return NotFound();
            var updatedType = new ProductType()
            {
                Id = id,
                Name = typeDTO.name,
                Description = typeDTO.description,
            };

            unit.TypeRepository.Update(updatedType);
            unit.SaveChanges();
            return Ok(updatedType);
        }
    }
}

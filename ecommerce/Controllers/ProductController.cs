﻿using ecommerce.DTO;
using ecommerce.HelperClasses;
using ecommerce.Models;
using ecommerce.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		UnitOfWork unit;
		IConfiguration configuration {  get; }
        Mapper mapper { get; }
        public ProductController(UnitOfWork unit,IConfiguration configuration,Mapper mapper)
        {
			this.unit = unit;
			this.configuration = configuration;
            this.mapper = mapper;
        }
        [HttpGet]
		public IActionResult Get([FromQuery] int page=1, [FromQuery] int pageSize=1)
		{
			if (page < 1 || pageSize < 1)
			return BadRequest("Invalid page or pageSize value.");
			
			var products = unit.ProductRepository.GetAll();
            var totalPagesNum = Math.Ceiling((decimal)products.Count()/pageSize);
			int itemsToSkip = (page - 1) * pageSize ;
			var productsPage = products.Skip(itemsToSkip).Take(pageSize).ToList();
			if (productsPage.Count() == 0) return NotFound();
			List<ProductDTO> productsDTOPage = mapper.ProductToDTO(productsPage);
			return Ok(new {
                currentPage= page,
                totalPages= totalPagesNum,
                products=productsDTOPage 
            });
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
            var p = unit.ProductRepository.GetById(id, "ProductType", "ProductCategory");
			if (p == null) return NotFound();
            var productDTO = mapper.productToUpdateDTO(p);
            return Ok(productDTO);
        }
		[HttpPost]
		public async Task<IActionResult> Add( [FromForm]ProductPostDTO productDTO) {

            if (productDTO.Image != null)
			{
				var imageName = await SaveImage(productDTO.Image);
				if (imageName != null)
				{
					var product = mapper.ProductPostDTOtoProduct(productDTO, imageName);
					unit.ProductRepository.Insert(product);
					unit.SaveChanges();
					var p = unit.ProductRepository.GetById(product.Id, "ProductType", "ProductCategory");
					var returnProductDTO = mapper.ProductToDTO(p);
					return Ok(returnProductDTO);
				}
			}
			return BadRequest("image not sent"); 
		}
        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            var imgName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(configuration.GetValue<string>("imgSavePath"), imgName);

            try
            {
                // Save the image using FileStream
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            catch
            {
                return null;
            }

            return imgName;
        }

        [HttpGet("/img/{imageName}")] // Specify that the endpoint expects a .jpg extension
        public IActionResult GetImage(string imageName)
        {
			var imagePath = Path.Combine(configuration.GetValue<string>("imgSavePath"), imageName); // Append .jpg extension to the image name
            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg"); // Specify the content type as image/jpeg
            }

            return NotFound();
        }

    }
}

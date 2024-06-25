using ecommerce.DTO;
using ecommerce.HelperClasses;
using ecommerce.Models;
using ecommerce.Repository;
using ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		UnitOfWork unit;
		IConfiguration configuration {  get; }
        Mapper mapper { get; }
        ProductService productService { get; }
        public ProductController(UnitOfWork unit,IConfiguration configuration,Mapper mapper,ProductService productService)
        {
			this.unit = unit;
			this.configuration = configuration;
            this.mapper = mapper;
            this.productService= productService;
        }
        [HttpGet("byuser")]
        [Authorize(Roles = "user")]
        public IActionResult GetByUser([FromQuery] int page=1, [FromQuery] int pageSize=1)
		{
			if (page < 1 || pageSize < 1)
			return BadRequest("Invalid page or pageSize value.");
			
			var products = unit.ProductRepository.GetAll();
            var totalPagesNum = Math.Ceiling((decimal)products.Count()/pageSize);
			int itemsToSkip = (page - 1) * pageSize ;
			var productsPage = products.Skip(itemsToSkip).Take(pageSize).ToList();
			if (productsPage.Count() == 0) return NotFound();
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<ProductDTO> productsDTOPage = productService.getProductsDTOByUser(productsPage, userIdClaim);
			return Ok(new {
                currentPage= page,
                totalPages= totalPagesNum,
                products=productsDTOPage 
            });
		}
        [HttpGet]
        public IActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid page or pageSize value.");

            var products = unit.ProductRepository.GetAll();
            var totalPagesNum = Math.Ceiling((decimal)products.Count() / pageSize);
            int itemsToSkip = (page - 1) * pageSize;
            var productsPage = products.Skip(itemsToSkip).Take(pageSize).ToList();
            if (productsPage.Count() == 0) return NotFound();
            List<ProductDTO> productsDTOPage = mapper.ProductToDTO(productsPage);
            return Ok(new
            {
                currentPage = page,
                totalPages = totalPagesNum,
                products = productsDTOPage
            });
        }

        [HttpGet("search")]
        public IActionResult SearchByName([FromQuery] string SearchWord,[FromQuery] int page = 1, [FromQuery] int pageSize = 1)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid page or pageSize value.");

            SearchWord=SearchWord.ToLower().Replace(" ","");
            var products = unit.ProductRepository.GetAll().Where(
                p=>p.Name.Replace(" ", "").ToLower().Contains(SearchWord)||
                p.ProductCategory.Name.Replace(" ", "").ToLower().Contains(SearchWord)|| 
                p.ProductType.Name.Replace(" ", "").ToLower().Contains(SearchWord)
                );
            var totalPagesNum = Math.Ceiling((decimal)products.Count() / pageSize);
            int itemsToSkip = (page - 1) * pageSize;
            var productsPage = products.Skip(itemsToSkip).Take(pageSize).ToList();
            List<ProductDTO> productsDTOPage = mapper.ProductToDTO(productsPage);
            return Ok(new
            {
                currentPage = page,
                totalPages = totalPagesNum,
                products = productsDTOPage
            });
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var p = unit.ProductRepository.GetById(id, "ProductType", "ProductCategory");
            if (p == null) return NotFound();
            var productDTO = mapper.ProductToDTO(p);
            return Ok(productDTO);
        }
        [HttpGet("{id}/byuser")]
        [Authorize(Roles ="user")]
        public IActionResult GetByIdByUser(int id)
        {
            var p = unit.ProductRepository.GetById(id, "ProductType", "ProductCategory");
            if (p == null) return NotFound();
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var productDTO = productService.getProductDTOByUser(p,userIdClaim);
            return Ok(productDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id , [FromForm] ProductPostDTO productDTO)
        {
            
            var product = unit.ProductRepository.GetById(id);
            if (product == null) return NotFound();
            var updatedProduct = new Product()
            {
                Id = id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Sale = productDTO.Sale,
                Stock = productDTO.Stock,
                TypeId = productDTO.TypeId,
                CategoryId = productDTO.CategoryId,
                ImageURL=product.ImageURL,
                DateCreated=product.DateCreated
                
            };
            if (productDTO.Image != null)
            {
                var imageName = await SaveImage(productDTO.Image);
                updatedProduct.ImageURL = "img/" + imageName;
            }
            unit.ProductRepository.Update(updatedProduct);
            unit.SaveChanges();
            return Ok(updatedProduct);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = unit.ProductRepository.GetById(id);
            if(product != null)
            {
                product.isDeleted = true;
                unit.ProductRepository.Update(product);
                unit.SaveChanges();
                return Ok();
            }
            return NotFound();
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

        [HttpGet("similar/{id}")]
        public IActionResult GetSimilar( int id ,int size=4 )
        {
       
            var product = unit.ProductRepository.GetById(id);
            if (product==null|| size < 1)
                return BadRequest("Invalid page or pageSize value.");

            var products = unit.ProductRepository.GetAll(p=>p.Id!=id&&(p.CategoryId==product.CategoryId));
            var productsPage = products.Take(size).ToList();
            if (productsPage.Count() < size)
            {

                var general = unit.ProductRepository.GetAll(p=>p.Id!=product.Id).Take(size).ToList();
                if(general.Count() == 0)
                {
                    return BadRequest();
                }
                var currentSize = productsPage.Count();
                for (int i = 0; i < size - currentSize; i++)
                {
                    productsPage.Add(general[i]);

                }

            }
            List<ProductDTO> productsDTOPage = mapper.ProductToDTO(productsPage);
            return Ok(  productsDTOPage );
        }

    }
}

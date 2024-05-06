using ecommerce.DTO;
using ecommerce.Models;
namespace ecommerce.HelperClasses
{
    public class Mapper
    {
        IConfiguration configuration { get; }

        public Mapper(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        public ProductDTO ProductToDTO(Product product)
        {
            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.ProductType.Name,
                Category = product.ProductCategory.Name,
                Price = product.Price,
                Sale = product.Sale,
                Stock = product.Stock,
                DateCreated = product.DateCreated,
                Description = product.Description,
                ImageURL = configuration.GetValue<string>("MainHost") + product.ImageURL,
            };
        }
        public List<ProductDTO> ProductToDTO(List<Product> productsList)
        {
            List<ProductDTO> productsDTOList = new List<ProductDTO>();
            foreach (var product in productsList)
            {
                productsDTOList.Add(ProductToDTO(product));
            }
            return productsDTOList;
        }

        public Product ProductPostDTOtoProduct(ProductPostDTO productPostDTO,string imgName) {
            return new Product()
            {
                Name = productPostDTO.Name,
                Description = productPostDTO.Description,
                CategoryId = productPostDTO.CategoryId,
                TypeId = productPostDTO.TypeId,
                Price = productPostDTO.Price,
                Sale = productPostDTO.Sale,
                Stock = productPostDTO.Stock,
                ImageURL = "img/" + imgName,
                DateCreated = DateTime.Now.Date,

            };
        }

        public ProductUpdateDTO productToUpdateDTO(Product product)
        {
            return new ProductUpdateDTO()
            {
                Name = product.Name,
                TypeId = product.ProductType.Id,
                CategoryId = product.ProductCategory.Id,
                Price = product.Price,
                Sale = product.Sale,
                Stock = product.Stock,
                Description = product.Description,
                Image = configuration.GetValue<string>("MainHost") + product.ImageURL,
            };
        }

    }
}

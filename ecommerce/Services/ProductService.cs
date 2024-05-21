using ecommerce.DTO;
using ecommerce.HelperClasses;
using ecommerce.Models;
using ecommerce.Repository;

namespace ecommerce.Services
{
    public class ProductService
    {
        Mapper mapper { get; }
        UnitOfWork unit {  get; }
        public ProductService(UnitOfWork unit,Mapper mapper)
        {
            this.unit=unit;
            this.mapper = mapper;
        }
        public List<ProductDTO> getProductsDTOByUser(List<Product> productsList,Guid userId)
        { 
           List<ProductDTO> productsDTO = new List<ProductDTO>();
            foreach (var product in productsList) {
                var productDTO = mapper.ProductToDTO(product);
                var cartproduct = unit.UsersCartsRepository.GetFirstByFilter(p => p.UserId == userId && p.ProductId==product.Id);
                if (cartproduct != null)
                {
                    productDTO.isAddedToCart = true;
                }
                else
                {
                    productDTO.isAddedToCart = false;
                }
                productsDTO.Add(productDTO);
            }
            return productsDTO;
        }
        public ProductDTO getProductDTOByUser(Product product, Guid userId)
        {
            var productDTO = mapper.ProductToDTO(product);
            var cartproduct = unit.UsersCartsRepository.GetFirstByFilter(p => p.UserId == userId && p.ProductId == product.Id);
            if (cartproduct != null)
            {
                productDTO.isAddedToCart = true;
            }
            else
            {
                productDTO.isAddedToCart = false;
            }
            return productDTO;
        }

    }
}

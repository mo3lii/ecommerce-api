using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Repository;

namespace ecommerce.Services
{
    public class OrderService
    {
        private UnitOfWork unit;
        public OrderService(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public List<UserCart> GetOrderCartProducts(Guid userId)
        {
            return unit.UsersCartsRepository.GetAll(c=>c.UserId==userId && c.Product.Stock>=c.Quantity).ToList();
        }
        public void AddOrder(OrderPostDTO postDTO, Guid userId)
        {
     
            Order order = new Order() {
                Government = postDTO.government,
                City = postDTO.city,
                Address = postDTO.address,
                Phone = postDTO.phone,
                Status = "confirmed",
                ShippingDate = DateTime.Now,
                UserId = userId,
            };
            unit.OrdersRepository.Insert(order);
            unit.SaveChanges();
            var Products = GetOrderCartProducts(userId);
            
            foreach(var product in Products)
            {
                var orderProducts = new OrderProduct() { 
                    OrderId = order.Id,
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                };
                unit.OrdersProductsRepository.Insert(orderProducts);
            }
            unit.SaveChanges();
        }
        public List<UserCart> getOrderProductsDetails(Guid userId)
        {
            return unit.UsersCartsRepository.GetAll(c => c.UserId == userId && c.Product.Stock >= c.Quantity,"Product").ToList();
        }

    }
}

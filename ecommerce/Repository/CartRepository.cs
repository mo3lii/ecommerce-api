using ecommerce.DTO;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repository
{
    public class CartRepository : GenericRepository<UserCart, int>
    {
        ecommerceContext _context;

        public CartRepository(ecommerceContext context) : base(context)
        {
            _context = context;
        }
        public List<UserCart> GetAllByUserId(Guid userId)
        {
            return _context.UsersCarts
                .Include(c=>c.Product)
                .Include(c=>c.User)
                .Include(c=>c.Product.ProductCategory)
                .Include(c=>c.Product.ProductType)
                .Where(e => e.UserId== userId && e.isDeleted == false && e.isActive == true).ToList();
        }
        

    }
}

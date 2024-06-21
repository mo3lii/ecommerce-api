using ecommerce.Models;
using Microsoft.Identity.Client;

namespace ecommerce.Repository
{
	public class UnitOfWork
	{
		private ecommerceContext context;
		private GenericRepository<Product,int> productRepository;
		private GenericRepository<ProductCategory,int> categoryRepository;
		private GenericRepository<ProductType,int> typeRepository;
		private GenericRepository<AppUser,Guid> usersRepository;
		private CartRepository usersCartsRepository;
        private GenericRepository<Order, int> ordersRepository;
        private GenericRepository<OrderProduct, int> ordersProductsRepository;

        public UnitOfWork(ecommerceContext _context)
		{
			context = _context;
		}
		public GenericRepository<Product,int> ProductRepository
		{
			get
			{
				if(productRepository == null)
				{
					productRepository = new GenericRepository<Product,int>(context);
				}
				return productRepository;
			}			
		}
		public GenericRepository<ProductCategory,int> CategoryRepository
		{
			get
			{
				if (categoryRepository == null)
				{
					categoryRepository = new GenericRepository<ProductCategory,int>(context);
				}
				return categoryRepository;
			}
		}
		public GenericRepository<ProductType,int> TypeRepository
		{
			get
			{
				if (typeRepository == null)
				{
					typeRepository = new GenericRepository<ProductType,int>(context);
				}
				return typeRepository;
			}
		}
        public GenericRepository<AppUser, Guid> UserRepository
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new GenericRepository<AppUser, Guid>(context);
                }
                return usersRepository;
            }
        }

        public CartRepository UsersCartsRepository
        {
            get
            {
                if (usersCartsRepository == null)
                {
                    usersCartsRepository = new CartRepository(context);
                }
                return usersCartsRepository;
            }
        }

        public GenericRepository<Order, int> OrdersRepository
        {
            get
            {
                if (ordersRepository == null)
                {
                    ordersRepository = new GenericRepository<Order, int>(context);
                }
                return ordersRepository;
            }
        }
        public GenericRepository<OrderProduct, int> OrdersProductsRepository
        {
            get
            {
                if (ordersProductsRepository == null)
                {
                    ordersProductsRepository = new GenericRepository<OrderProduct, int>(context);
                }
                return ordersProductsRepository;
            }
        }
        public void SaveChanges()
		{
			context.SaveChanges();
		}
	}
}

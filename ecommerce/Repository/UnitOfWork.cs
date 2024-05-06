using ecommerce.Models;
using Microsoft.Identity.Client;

namespace ecommerce.Repository
{
	public class UnitOfWork
	{
		private ecommerceContext context;
		private GenericRepository<Product> productRepository;
		private GenericRepository<ProductCategory> categoryRepository;
		private GenericRepository<ProductType> typeRepository;
		public UnitOfWork(ecommerceContext _context)
		{
			context = _context;
		}
		public GenericRepository<Product> ProductRepository
		{
			get
			{
				if(productRepository == null)
				{
					productRepository = new GenericRepository<Product>(context);
				}
				return productRepository;
			}			
		}
		public GenericRepository<ProductCategory> CategoryRepository
		{
			get
			{
				if (categoryRepository == null)
				{
					categoryRepository = new GenericRepository<ProductCategory>(context);
				}
				return categoryRepository;
			}
		}
		public GenericRepository<ProductType> TypeRepository
		{
			get
			{
				if (typeRepository == null)
				{
					typeRepository = new GenericRepository<ProductType>(context);
				}
				return typeRepository;
			}
		}
		public void SaveChanges()
		{
			context.SaveChanges();
		}
	}
}

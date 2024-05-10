using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
	public class ecommerceContext:DbContext
	{
        public ecommerceContext(){    }
        public ecommerceContext(DbContextOptions<ecommerceContext> options):base(options) { }
        
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	base.OnConfiguring(optionsBuilder);
		//}

		public DbSet<Product> Products { get; set; } 
		public DbSet<ProductCategory> Categories { get; set; }
		public DbSet<ProductType> Types { get; set; }
	}
}

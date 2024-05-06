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

		DbSet<Product> Products { get; set; } 
		DbSet<ProductCategory> Categories { get; set; }
		DbSet<ProductType> Types { get; set; }
	}
}

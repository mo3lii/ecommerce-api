using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class ProductCategory : IEntity<int>
    {
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }

		public virtual ICollection<Product> Products { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isActive { get; set; } = true;


    }
}

using ecommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public int typeId { get; set; }
		public string Category { get; set; }
		public int categoryId { get; set; }
        public string Description { get; set; }
		public string ImageURL { get; set; }
		public decimal Price { get; set; }
		public int? Sale { get; set; }
		public int Stock { get; set; }
		public bool isAddedToCart { get; set; } = false;
		public DateTime? DateCreated { get; set; }

	}
}

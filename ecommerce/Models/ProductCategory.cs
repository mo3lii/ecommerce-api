﻿using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class ProductCategory : IEntity
    {
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }

		public virtual ICollection<Product> Products { get; set; }

	}
}
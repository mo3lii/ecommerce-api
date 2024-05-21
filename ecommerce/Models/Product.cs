using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
	public class Product:IEntity<int>
	{
        [Key]
		public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("ProductType")]
        public int TypeId { get; set; }

		[ForeignKey("ProductCategory")]
		public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		public int? Sale { get; set; }
        public int Stock { get; set; }
        public DateTime? DateCreated { get; set; }
        
        public bool isDeleted { get; set; }=false;
        public bool isActive { get; set; } = true;

        //reference for FK
        public virtual ProductType ProductType { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<UserCart> usercarts { get; set;}


    }
}

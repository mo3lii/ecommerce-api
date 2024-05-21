using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class UserCart : IEntity<int>
    {
        [Key]
        public int Id { get ; set; }
        [ForeignKey("User")]
        public Guid UserId {  get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public bool isDeleted { get; set; } = false;
        public bool isActive { get; set; } = true;

        virtual public Product Product { get; set; }
        virtual public AppUser User { get; set; }
    }
}

using System.Numerics;

namespace ecommerce.Models
{
    public class Order:IEntity<int>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Government {  get; set; }
        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime ShippingDate {  get; set; }
        public string Status {  get; set; }

        public bool isDeleted { get ; set; }=false;
        public bool isActive { get; set; } = true;

        public virtual AppUser User { get; set; }
        public virtual List<OrderProduct> Products {  get; set; }

    }
}

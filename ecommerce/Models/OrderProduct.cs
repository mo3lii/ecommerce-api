namespace ecommerce.Models
{
    public class OrderProduct : IEntity<int>
    {
        public int Id { get ; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
        public bool isDeleted { get; set; }
        public bool isActive { get ; set ; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

    }
}

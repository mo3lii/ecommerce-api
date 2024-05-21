namespace ecommerce.Models
{
    public interface IEntity<T> 
    {
        T Id { get; set; }
        bool isDeleted { get; set; }
        bool isActive {  get; set; }
    }
}

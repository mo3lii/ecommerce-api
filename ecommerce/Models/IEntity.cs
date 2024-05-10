namespace ecommerce.Models
{
    public interface IEntity
    {
        int Id { get; }
        bool isDeleted { get; set; }
        bool isActive {  get; set; }
    }
}

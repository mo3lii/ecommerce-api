namespace ecommerce.DTO
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string imageURL { get; set; }
        public decimal Price { get; set; }
        public int? Sale { get; set; }
        public int Stock { get; set; }
    }
}

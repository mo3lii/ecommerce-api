namespace ecommerce.DTO
{
    public class ProductByUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int? Sale { get; set; }
        public int Stock { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}

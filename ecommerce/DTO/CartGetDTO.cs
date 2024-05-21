using ecommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.DTO
{
    public class CartGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ProductId {  get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterSale { get {
                if (Sale!=null && Sale.Value > 0)
                {
                    return Price * (1 - (Sale / 100m));
                }
                else
                {
                    return Price;
                }
          
            } }
        public int? Sale { get; set; }
        public int Stock { get; set; }

        public int quantity { get; set;}
    }
}

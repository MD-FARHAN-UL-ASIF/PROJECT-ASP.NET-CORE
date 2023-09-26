using System.ComponentModel.DataAnnotations.Schema;

namespace HealthRX.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IFormFile PictureLink { get; set; }
    }
}

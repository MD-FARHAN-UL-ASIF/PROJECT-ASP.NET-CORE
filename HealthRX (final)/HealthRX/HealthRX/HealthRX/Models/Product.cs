using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthRX.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Catagory")]
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PictureLink { get; set; }
        public virtual Catagory Catagory { get; set; }
    }
}

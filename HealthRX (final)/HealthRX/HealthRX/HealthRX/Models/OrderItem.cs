using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthRX.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal ItemCost { get; set; }

        public virtual Product Product { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthRX.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        public string Address { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HealthRX.Models
{
    public class Catagory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

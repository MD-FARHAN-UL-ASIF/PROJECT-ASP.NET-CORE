using System.ComponentModel.DataAnnotations;

namespace HealthRX.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public int LoacationId { get; set; }
        [ForeignKey("Hospital")]
        public int HospitalId { get; set; }
        public string Review { get; set; }
        public int Fee { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual Category Category { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}

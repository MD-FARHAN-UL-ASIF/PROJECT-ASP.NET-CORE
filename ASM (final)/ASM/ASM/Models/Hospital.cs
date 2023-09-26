using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public string Code { get; set; }
        public virtual Location Location { get; set; }
    }
}

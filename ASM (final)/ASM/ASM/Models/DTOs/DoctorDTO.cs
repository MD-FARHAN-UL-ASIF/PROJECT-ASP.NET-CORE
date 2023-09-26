using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models.DTOs
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CatagoryName { get; set; }
        public string HospitalName { get; set; }
        public string LocationName { get; set; }
        public string Password { get; set; }
    }
}

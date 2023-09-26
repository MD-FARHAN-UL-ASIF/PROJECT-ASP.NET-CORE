namespace ASM.Models.DTOs
{
    public class DoctorHospitalLocationDTO
    {
        public Doctor Doctorz { get; set; }
        public List<Location> Locationz { get; set; }
        public List<Hospital> Hospitalz { get; set; }
        public List<Category> Categoriez { get; set; }
    }
}

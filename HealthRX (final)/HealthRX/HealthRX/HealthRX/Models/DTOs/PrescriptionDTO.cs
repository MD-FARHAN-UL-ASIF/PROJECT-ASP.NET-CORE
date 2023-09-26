namespace HealthRX.Models.DTOs
{
    public class PrescriptionDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public IFormFile PictureLink { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}

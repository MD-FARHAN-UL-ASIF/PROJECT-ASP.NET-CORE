namespace HealthRX.Models
{
    public class Prescrition
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PictureLink { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}

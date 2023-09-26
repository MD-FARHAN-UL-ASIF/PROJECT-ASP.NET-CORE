namespace ASM.Models.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string LocationName { get; set; }
        public string HospitalName { get; set; }
        public string CategoryName { get; set; }
        public string DoctorName { get; set; }
        public string Note { get; set; }

    }
}

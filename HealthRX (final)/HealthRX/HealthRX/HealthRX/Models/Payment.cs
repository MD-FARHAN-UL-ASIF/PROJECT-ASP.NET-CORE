namespace HealthRX.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string TrxId { get; set; }
    }
}

namespace HealthRX.Models.DTOs
{
    public class CatagoryProductDTO : CatagoryDTO
    {
        public List<ProductDTO> Products { get; set; }
        public CatagoryProductDTO()
        {
            Products = new List<ProductDTO>();
        }
    }
}

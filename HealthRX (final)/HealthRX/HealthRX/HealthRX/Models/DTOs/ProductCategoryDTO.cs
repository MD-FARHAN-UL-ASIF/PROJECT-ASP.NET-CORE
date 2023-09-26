namespace HealthRX.Models.DTOs
{
    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PictureLink { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set;}
        public List<CatagoryDTO> CatagoryDTOs { get; set; }

    }
}

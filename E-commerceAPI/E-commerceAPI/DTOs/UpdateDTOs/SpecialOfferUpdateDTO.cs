namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class SpecialOfferUpdateDTO
    {
        public int Id { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

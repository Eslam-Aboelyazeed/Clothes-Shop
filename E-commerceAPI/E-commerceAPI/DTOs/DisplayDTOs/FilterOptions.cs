namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class FilterOptions
    {
        public bool TShirts { get; set; }
        public bool Shoes { get; set; }
        public bool Jackets { get; set; }
        public bool Other { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }
        public bool FiveStars { get; set; }
        public bool FourStars { get; set; }
        public bool ThreeStars { get; set; }
        public bool TwoStars { get; set; }
        public bool OneStar { get; set; }
        public string Name { get; set; }
    }
}

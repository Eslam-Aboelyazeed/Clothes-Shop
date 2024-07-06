namespace E_commerceAPI.DTOs.InsertDTOs
{
    public class ApplicationUserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}

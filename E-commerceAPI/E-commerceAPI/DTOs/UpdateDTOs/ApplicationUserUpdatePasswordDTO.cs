namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class ApplicationUserUpdatePasswordDTO
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

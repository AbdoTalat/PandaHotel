using System.ComponentModel.DataAnnotations;


namespace HotelApp.Application.DTOs.Account
{
    public class LoginDTO
    {
        [EmailAddress]
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}

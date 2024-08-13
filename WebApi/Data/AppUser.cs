using Microsoft.AspNetCore.Identity;

namespace WebApi.Data
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImgPath { get; set; }
    }
}

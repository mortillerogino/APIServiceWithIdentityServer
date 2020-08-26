using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string ReferrerId { get; set; }
    }
}

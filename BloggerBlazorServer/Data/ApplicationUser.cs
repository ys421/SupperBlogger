using Microsoft.AspNetCore.Identity;

namespace BloggerBlazorServer.Data;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = false;
}

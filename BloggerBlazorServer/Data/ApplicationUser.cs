using Microsoft.AspNetCore.Identity;

namespace BloggerBlazorServer.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // 새로 추가한 필드: 계정 활성 여부 (true이면 활성, false이면 비활성)
    public bool IsActive { get; set; } = false;
}

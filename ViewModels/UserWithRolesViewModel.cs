using MLFamilyTravelBlog.Models;

namespace MLFamilyTravelBlog.ViewModels
{
    public class UserWithRolesViewModel
    {
        public BlogUser? User { get; set; }
        public List<string>? Roles { get; set; }
    }
}

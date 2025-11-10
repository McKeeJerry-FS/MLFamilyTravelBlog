using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLFamilyTravelBlog.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters or less than 2 characters.", MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters or less than 2 characters.", MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Bio cannot be longer than 1000 characters.")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters.")]
        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        [StringLength(200)]
        [Display(Name = "Facebook URL")]
        public string? FacebookUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Instagram URL")]
        public string? InstagramUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Twitter URL")]
        public string? TwitterUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedInUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "GitHub URL")]
        public string? GitHubUrl { get; set; }

        [NotMapped]
        public string? FullName { get 
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                {
                    return $"{FirstName} {LastName}";
                }
                return null;
            }
        }

        // Image Properties
        [NotMapped]
        public IFormFile? ImageData { get; set; }
        public byte[]? ImageType { get; set; }
        public string? ImageFile { get; set; }

        // Navigation Properties (Plural)
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<BlogLike> Likes { get; set; } = new HashSet<BlogLike>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace MLFamilyTravelBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Body { get; set; } = string.Empty;
        [Display(Name = "Created At")]
        public DateTimeOffset CreatedAt { get; set; }
        [Display(Name = "Updated At")]
        public DateTimeOffset Updated { get; set; }
        public string? UpdateReason { get; set; }
        
        // Foreign Keys
        public string AuthorId { get; set; } = string.Empty;
        public int BlogPostId { get; set; }


        // Navigation Properties
        public virtual BlogUser? Author { get; set; }
        public virtual BlogPost? BlogPost { get; set; }
    }
}

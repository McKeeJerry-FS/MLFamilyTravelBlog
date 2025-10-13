using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLFamilyTravelBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least{2} and at most {1} characters.", MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;
        [StringLength(600, ErrorMessage = "The {0} must be at least{2} and at most {1} characters.", MinimumLength = 2)]
        public string? Abstract { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        [Display(Name = "Created Date")]
        public DateTimeOffset Created { get; set; }
        [Display(Name = "Updated Date")]
        public DateTimeOffset? Updated { get; set; }
        [Display(Name = "Revival Date")]
        public DateTimeOffset RevivalDate { get; set; }
        public string? Slug { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public bool IsArchived { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public int ViewCount { get; set; }

        // Image Properties
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        // Navigation Properties (Singular)
        public virtual Category? Category { get; set; }

        // Navigation Properties (Plural)
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<BlogLike> Likes { get; set; } = new HashSet<BlogLike>();

    }
}

using System.ComponentModel.DataAnnotations;

namespace MLFamilyTravelBlog.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public DateTime SubscribedOn { get; set; }
    }
}

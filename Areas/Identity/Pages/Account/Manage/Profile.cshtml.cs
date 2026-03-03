#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLFamilyTravelBlog.Models;
using MLFamilyTravelBlog.Services.Interfaces;

namespace MLFamilyTravelBlog.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly SignInManager<BlogUser> _signInManager;
        private readonly IImageService _imageService;

        public ProfileModel(
            UserManager<BlogUser> userManager,
            SignInManager<BlogUser> signInManager,
            IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string CurrentImageUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters or less than 2 characters.", MinimumLength = 2)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters or less than 2 characters.", MinimumLength = 2)]
            public string LastName { get; set; }

            [StringLength(1000, ErrorMessage = "Bio cannot be longer than 1000 characters.")]
            [Display(Name = "Bio")]
            [DataType(DataType.MultilineText)]
            public string Bio { get; set; }

            [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters.")]
            [Display(Name = "Job Title")]
            public string JobTitle { get; set; }

            [StringLength(200)]
            [Display(Name = "Facebook URL")]
            [Url]
            public string FacebookUrl { get; set; }

            [StringLength(200)]
            [Display(Name = "Instagram URL")]
            [Url]
            public string InstagramUrl { get; set; }

            [StringLength(200)]
            [Display(Name = "Twitter URL")]
            [Url]
            public string TwitterUrl { get; set; }

            [StringLength(200)]
            [Display(Name = "LinkedIn URL")]
            [Url]
            public string LinkedInUrl { get; set; }

            [StringLength(200)]
            [Display(Name = "GitHub URL")]
            [Url]
            public string GitHubUrl { get; set; }

            [Display(Name = "Profile Image")]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(BlogUser user)
        {
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                JobTitle = user.JobTitle,
                FacebookUrl = user.FacebookUrl,
                InstagramUrl = user.InstagramUrl,
                TwitterUrl = user.TwitterUrl,
                LinkedInUrl = user.LinkedInUrl,
                GitHubUrl = user.GitHubUrl
            };

            // Generate current image URL for preview
            CurrentImageUrl = _imageService.ConvertByteArrayToFile(
                user.ImageType, 
                user.ImageFile, 
                Models.Enums.DefaultImage.AuthorImage);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Update user profile
            bool hasChanges = false;

            if (user.FirstName != Input.FirstName)
            {
                user.FirstName = Input.FirstName;
                hasChanges = true;
            }

            if (user.LastName != Input.LastName)
            {
                user.LastName = Input.LastName;
                hasChanges = true;
            }

            if (user.Bio != Input.Bio)
            {
                user.Bio = Input.Bio;
                hasChanges = true;
            }

            if (user.JobTitle != Input.JobTitle)
            {
                user.JobTitle = Input.JobTitle;
                hasChanges = true;
            }

            if (user.FacebookUrl != Input.FacebookUrl)
            {
                user.FacebookUrl = Input.FacebookUrl;
                hasChanges = true;
            }

            if (user.InstagramUrl != Input.InstagramUrl)
            {
                user.InstagramUrl = Input.InstagramUrl;
                hasChanges = true;
            }

            if (user.TwitterUrl != Input.TwitterUrl)
            {
                user.TwitterUrl = Input.TwitterUrl;
                hasChanges = true;
            }

            if (user.LinkedInUrl != Input.LinkedInUrl)
            {
                user.LinkedInUrl = Input.LinkedInUrl;
                hasChanges = true;
            }

            if (user.GitHubUrl != Input.GitHubUrl)
            {
                user.GitHubUrl = Input.GitHubUrl;
                hasChanges = true;
            }

            // Handle profile image upload
            if (Input.ProfileImage != null)
            {
                user.ImageType = await _imageService.ConvertFileToByteArrayAsynC(Input.ProfileImage);
                user.ImageFile = Input.ProfileImage.ContentType;
                hasChanges = true;
            }

            if (hasChanges)
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Error: Unable to update profile.";
                    return RedirectToPage();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your author profile has been updated successfully!";
            }
            else
            {
                StatusMessage = "No changes were made to your profile.";
            }

            return RedirectToPage();
        }
    }
}
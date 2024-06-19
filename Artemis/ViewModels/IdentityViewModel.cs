using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Artemis.ViewModels
{
    public class AppUserViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First name")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Address")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string? Address { get; set; }

        [Display(Name = "Phone number")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Country")]
        [MaxLength(256, ErrorMessage = "Maximum allowed length is 256 characters")]
        public string? Country { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get => FirstName + " " + LastName;
        }

        public int UsernameChangeLimit { get; set; } = 10;

        public bool EmailConfirmed { get; set; } = true;
        public bool PhoneConfirmed { get; set; } = true;
        public byte[]? ProfilePicture { get; set; }

        public string? ProfileImage { get; set; }

        public IEnumerable<ApplicationUserRole> RolesNavigation { get; set; } = Enumerable.Empty<ApplicationUserRole>();
        public IEnumerable<ApplicationUserClaim> ClaimsNavigation { get; set; } = Enumerable.Empty<ApplicationUserClaim>();
    }

    public class UserRoleViewModel
    {
        public string? Key { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }

    public class RoleViewModel
    {
        public string RoleId { get; set; } = Guid.NewGuid().ToString();
        public string? RoleName { get; set; }
        public string? RoleNameNormalized { get; set; }
    }

    public class UserClaimViewModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }

    public class UserSummaryViewModel
    {
        public int? TotalUsers { get; set; }
        public int? BasicUsers { get; set; }
        public int? Administrators { get; set; }
        public int? Moderators { get; set; }
    }

    public class UserChartModel
    {
        public string? UserType { get; set; }
        public decimal Percentage { get; set; }
        public bool Explode { get; set; }
        public string? Color { get; set; }


        public UserChartModel()
        { }

        public UserChartModel(string userType, decimal percentage, string? color = null, bool explode = false)
        {
            UserType = userType;
            Percentage = percentage;
            Color = color;
            Explode = explode;
        }
    }
}

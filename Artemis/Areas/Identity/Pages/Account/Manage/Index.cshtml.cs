// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Display(Name = "First Name"), MaxLength(256)]
            public string FirstName { get; set; }

            [Display(Name = "Last Name"), MaxLength(256)]
            public string LastName { get; set; }

            [Display(Name = "Username"), MaxLength(256)]
            public string UserName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date of birth")]
            public DateTime BirthDate { get; set; }

            [Display(Name = "Address"), MaxLength(256)]
            public string Address { get; set; }

            [Display(Name = "Country"), MaxLength(256)]
            public string Country { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                UserName = userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Country = user.Country,
                BirthDate = user.BirthDate ?? DateTime.Now.Date,
                ProfilePicture = user.ProfilePicture
            };
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

            if (Request.Form.Files.Any())
            {
                IFormFile file = Request.Form.Files[0];
                string extension = Path.GetExtension(file.FileName);
                if (file.Length > Common.IMAGE_MAXSIZE)
                {
                    ModelState.AddModelError(string.Empty, $"Uploaded image size must be less than {Common.IMAGE_MAXSIZE / 1024}KB.");
                }
                if (!Common.ValidImageExtension(extension))
                {
                    ModelState.AddModelError(string.Empty, $"Uploaded file extension '{extension}' is not accepted.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Country != user.Country)
            {
                user.Country = Input.Country.ToUpperInvariant();
                await _userManager.UpdateAsync(user);
            }
            if (Input.BirthDate != user.BirthDate)
            {
                user.BirthDate = Input.BirthDate;
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error: Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files[0];
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }
            // if user has provided Firstname, Lastname create claim
            bool hasFullname = User.HasClaim(u => u.Type == "FullName");
            if (!hasFullname)
            {
                var claim = new Claim("FullName", Input.FirstName + " " + Input.LastName);
                await _userManager.AddClaimAsync(user, claim);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Success: Your profile has been updated.";
            return RedirectToPage();
        }
    }
}

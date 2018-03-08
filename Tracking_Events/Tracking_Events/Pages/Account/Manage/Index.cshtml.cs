using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Events.Data;
using Tracking_Events.Services;
using System.Globalization;

namespace Tracking_Events.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Venue Name")]
            public string VenueName { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [Display(Name = "State")]
            public string State { get; set; }

            [Required]
            [Display(Name = "Zip")]
            public int Zip { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var venue = _context.Venue.SingleOrDefault(v => v.UserID == user.Id);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
                Email = user.Email,
                VenueName = venue.VenueName,
                Address = venue.Address,
                City = venue.City,
                State = venue.State,
                Zip = venue.Zip
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var venue = _context.Venue.SingleOrDefault(v => v.UserID == user.Id);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                var setUserResult = await _userManager.SetUserNameAsync(user, Input.Email);
                if (!setEmailResult.Succeeded || !setUserResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email/username for user with ID '{user.Id}'.");
                }
            }

            if (Input.VenueName != user.Venue.VenueName || Input.Address != user.Venue.Address || Input.City != user.Venue.City || Input.State != user.Venue.State || Input.Zip != user.Venue.Zip)
            {
                venue.VenueName = capitalize.ToTitleCase(Input.VenueName);
                venue.Address = capitalize.ToTitleCase(Input.Address);
                venue.City = capitalize.ToTitleCase(Input.City);
                venue.State = Input.State.ToUpper();
                venue.Zip = Input.Zip;

                var result = await _userManager.UpdateAsync(user);

                //Update Venue
                _context.Attach(venue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occured Updating for user with ID '{user.Id}'.");
                }
            }
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete()
        {
            
            return RedirectToPage("./DeleteConfirm");
        }
    }
}

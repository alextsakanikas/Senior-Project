using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tracking_Events.Data;
using System.Globalization;

namespace Tracking_Events.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Request Request { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _context.ApplicationUser.SingleOrDefault(u => u.Id == _userManager.GetUserAsync(User).Result.Id);
            
            Request.Venue = _context.Venue.SingleOrDefault(v => v.User.Id == user.Id);
            Request.EventName = capitalize.ToTitleCase(Request.EventName);
            Request.Genre = capitalize.ToTitleCase(Request.Genre);

            _context.Request.Add(Request);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { statusMessage = "Please wait for Admin Approval" });
        }
    }
}
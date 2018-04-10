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
using Microsoft.EntityFrameworkCore;

namespace Tracking_Events.Pages.Events
{
    public class ManageEventsCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public ManageEventsCreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            IQueryable<Venue> venues = _context.Venue.Include(v => v.User).Where(v => v.User.Id == _userManager.GetUserAsync(User).Result.Id).AsQueryable();
            VenueSelectList = venues.Select(v => new SelectListItem { Value = v.VenueID.ToString(), Text = v.VenueName });

            return Page();
        }

        [BindProperty]
        public Request Request { get; set; }
        public IEnumerable<SelectListItem> VenueSelectList { get; set; }

        [BindProperty]
        public string VenueID { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                IQueryable<Venue> venues = _context.Venue.Include(v => v.User).Where(v => v.User.Id == _userManager.GetUserAsync(User).Result.Id).AsQueryable();
                VenueSelectList = venues.Select(v => new SelectListItem { Value = v.VenueID.ToString(), Text = v.VenueName });
                return Page();
            }

            var user = _context.ApplicationUser.SingleOrDefault(u => u.Id == _userManager.GetUserAsync(User).Result.Id);
            
            Request.Venue = _context.Venue.Include(v => v.User).SingleOrDefault(v => v.VenueID == Convert.ToInt32(VenueID));
            Request.EventName = capitalize.ToTitleCase(Request.EventName);
            Request.Genre = capitalize.ToTitleCase(Request.Genre);

            _context.Request.Add(Request);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageEvents", new { statusMessage = "Please wait for Admin Approval" });
        }
    }
}
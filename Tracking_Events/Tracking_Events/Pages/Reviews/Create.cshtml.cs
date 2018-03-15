using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly Tracking_Events.Data.ApplicationDbContext _context;

        public CreateModel(Tracking_Events.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var venuelist = from s in _context.Venue
                             select s;
            VenueNames = venuelist.Select(v => new SelectListItem() { Value = v.VenueID.ToString(), Text = v.VenueName });
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; }
        public IEnumerable<SelectListItem> VenueNames { get; set; }

        [BindProperty]
        public string VenueID { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.Venue = _context.Venue.SingleOrDefault(v => v.VenueID == Convert.ToInt32(VenueID));
            Review.UserName = User.Identity.Name;

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
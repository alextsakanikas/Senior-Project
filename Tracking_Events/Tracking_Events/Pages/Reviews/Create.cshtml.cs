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
            var venuelist = (from s in _context.ApplicationUser
                             where s.AccountType == 1
                             select s).Distinct();
            VenueNames = venuelist.Select(v => new SelectListItem() { Value = v.VenueName, Text = v.VenueName });
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; }
        public IEnumerable<SelectListItem> VenueNames { get; set; }

        [BindProperty]
        public string VenueName { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.Venue = _context.ApplicationUser.SingleOrDefault(a => a.VenueName == VenueName);
            Review.UserName = User.Identity.Name;

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
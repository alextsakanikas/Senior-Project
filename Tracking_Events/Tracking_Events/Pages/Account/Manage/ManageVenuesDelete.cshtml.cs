using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageVenuesDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageVenuesDeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Venue Venue { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venue = await _context.Venue.Include(v => v.User).SingleOrDefaultAsync(v => v.VenueID == id);

            if (Venue == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Venue = await _context.Venue.FindAsync(id);

            if (Venue != null)
            {
                _context.Venue.Remove(Venue);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageVenues");
        }
    }
}

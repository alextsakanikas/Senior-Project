using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Tracking_Events.Pages.Account.Manage
{
    public enum VenueType
    {
        Official = 1, Personal
    }

    public class ManageVenuesEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public ManageVenuesEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Venue Venue { get; set; }

        [BindProperty]
        [Display(Name = "Venue Type")]
        public VenueType VenueType { get; set; }
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venue = await _context.Venue.Include(v => v.User).SingleOrDefaultAsync(m => m.VenueID == id);
            VenueType = (VenueType)Venue.VenueType;

            if (Venue == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Venue.VenueType = Convert.ToInt32(VenueType);

            _context.Attach(Venue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(Venue.VenueID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            StatusMessage = "You have successfully changed your settings";

            return RedirectToPage("./ManageVenuesEdit", new { id = Venue.VenueID });
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
    }
}

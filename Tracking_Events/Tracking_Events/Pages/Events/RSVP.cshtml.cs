using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Events
{
    public class RSVPModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RSVPModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Event Event = _context.Event.Where(e => e.EventID == id).SingleOrDefault();
            ApplicationUser user = await _userManager.GetUserAsync(User);

            RSVP rsvp = new RSVP
            {
                Event = Event,
                User = user
            };

            await _context.RSVP.AddAsync(rsvp);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { statusMessage = "You have successfully RSVPed" });
        }
    }
}
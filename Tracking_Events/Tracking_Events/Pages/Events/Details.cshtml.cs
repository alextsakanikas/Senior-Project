using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        //Created to get single event and use that to get user data
        public Event Event { get; set; }

        [BindProperty]
        public int RSVPAmount { get; set; }

        public IActionResult OnGet(int id)
        {
            //Used to get Parent and Foreign tables
            Event = _context.Event.Include(ev => ev.Venue).ThenInclude(v => v.User).Include(ev => ev.Rsvps).Where(e => e.EventID == id).SingleOrDefault();

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, int rsvpAmount)
        {
            Event Event = _context.Event.Where(e => e.EventID == id).SingleOrDefault();
            ApplicationUser user = await _userManager.GetUserAsync(User);

            for (int i = 0; i < rsvpAmount; i++)
            {
                RSVP rsvp = new RSVP
                {
                    Event = Event,
                    User = user
                };

                await _context.RSVP.AddAsync(rsvp);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { statusMessage = "You have successfully RSVPed" });
        }
    }
}

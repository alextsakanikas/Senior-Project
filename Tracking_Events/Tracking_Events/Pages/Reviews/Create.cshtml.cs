using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int eventId, int venueId)
        {
            Event = _context.Event.Include(e => e.Venue).SingleOrDefault(e => e.EventID == eventId);

            return Page();
        }

        [BindProperty]
        public Review Review { get; set; }

        [BindProperty]
        public Event Event { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.UserName = User.Identity.Name;
            Review.Venue = Event.Venue;
            Review.Event = Event;
            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Requests
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> Requests { get; set; }

        public async Task OnGetAsync()
        {
            var requests = _context.Request.Include(r => r.Venue).AsQueryable();

            Requests = await requests.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int id, string judgement)
        {
            var request = _context.Request.Include(r => r.Venue).SingleOrDefault(r => r.RequestID == id);

            if (judgement.Equals("Approve"))
            {
                Event Event = new Event
                {
                    Venue = request.Venue,
                    EventName = request.EventName,
                    Genre = request.Genre,
                    AgeRequirement = request.AgeRequirement,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Description = request.Description
                };

                await _context.Event.AddAsync(Event);
                _context.Request.Remove(request);
                await _context.SaveChangesAsync();
            }
            else if (judgement.Equals("Reject"))
            {
                _context.Request.Remove(request);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

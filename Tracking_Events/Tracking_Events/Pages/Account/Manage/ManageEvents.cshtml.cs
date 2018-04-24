using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageEventsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageEventsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Event> Event { get;set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(string statusMessage)
        {
            StatusMessage = statusMessage;

            var events = _context.Event.Include(e => e.Venue).ThenInclude(e => e.User).Where(e => e.Venue.User.Id == _userManager.GetUserAsync(User).Result.Id && e.EndTime > DateTime.Now).AsQueryable();
            Event = await events.OrderBy(e => e.EventName).ToListAsync();
        }
    }
}

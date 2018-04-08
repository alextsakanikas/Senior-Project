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
    public class ManageRSVPsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageRSVPsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<RSVP> RSVPs { get;set; }

        public async Task OnGetAsync()
        {
            RSVPs = await _context.RSVP.Include(r => r.Event).Include(r => r.User).Where(r => r.User.Id == _userManager.GetUserAsync(User).Result.Id).ToListAsync();
        }
    }
}

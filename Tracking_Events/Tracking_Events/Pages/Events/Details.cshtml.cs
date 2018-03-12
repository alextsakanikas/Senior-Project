using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //Created to get single event and use that to get user data
        public Event Event { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Used to get Parent and Foreign tables
            Event = _context.Event.Include(ev => ev.Venue).Where(e => e.EventID == Convert.ToInt32(id)).SingleOrDefault();

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

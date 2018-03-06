using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly Tracking_Events.Data.ApplicationDbContext _context;

        public IndexModel(Tracking_Events.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Reviews { get;set; }

        public async Task OnGetAsync()
        {
            Reviews = await _context.Review.Include(r => r.Venue).ToListAsync();
        }
    }
}

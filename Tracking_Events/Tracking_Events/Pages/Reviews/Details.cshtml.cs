﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class DetailsModel : PageModel
    {
        private readonly Tracking_Events.Data.ApplicationDbContext _context;

        public DetailsModel(Tracking_Events.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review = await _context.Review.Include(r => r.Venue).SingleOrDefaultAsync(m => m.ReviewID == Convert.ToInt32(id));

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

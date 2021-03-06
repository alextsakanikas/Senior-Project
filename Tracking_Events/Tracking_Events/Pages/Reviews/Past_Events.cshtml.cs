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
    public class Past_EventsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Past_EventsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Event> Event { get;set; }

        public async Task OnGetAsync()
        {
            Event = await _context.Event.Include(ev => ev.Venue).ThenInclude(v => v.User).Include(ev => ev.Rsvps).Where(e => e.EndTime < DateTime.Now).ToListAsync();
        }
    }
}

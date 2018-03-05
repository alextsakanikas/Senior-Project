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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Event> Event { get; set; }

        public string CurrentFilter { get; set; }

        #region Sorting Purposes
        public string EventNameSort { get; set; }
        public string VenueNameSort { get; set; }
        public string StartTimeSort { get; set; }
        public string EndTimeSort { get; set; }
        #endregion

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            EventNameSort = String.IsNullOrEmpty(sortOrder) ? "event_name_desc" : "";
            VenueNameSort = sortOrder == "Venue" ? "venue_desc" : "Venue";
            StartTimeSort = sortOrder == "Starttime" ? "start_time_desc" : "Starttime";
            EndTimeSort = sortOrder == "Endtime" ? "end_time_desc" : "Endtime";

            //Delete Events 3 hours past expire time
            _context.Event.RemoveRange(_context.Event.Where(e => e.EndTime < DateTime.Now.AddHours(3)));
            await _context.SaveChangesAsync();

            IQueryable<Event> events = _context.Event.Include(ev => ev.User).AsQueryable();

            #region Filtering
            CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.User.VenueName.Contains(searchString) || s.Genre.Contains(searchString) || s.User.Zip.ToString().Equals(searchString));
            }
            #endregion

            switch (sortOrder)
            {
                case "event_name_desc":
                    events = events.OrderByDescending(s => s.EventName);
                    break;
                case "Venue":
                    events = events.OrderBy(s => s.User.VenueName);
                    break;
                case "venue_desc":
                    events = events.OrderByDescending(s => s.User.VenueName);
                    break;
                case "Starttime":
                    events = events.OrderBy(s => s.StartTime);
                    break;
                case "start_time_desc":
                    events = events.OrderByDescending(s => s.StartTime);
                    break;
                case "Endtime":
                    events = events.OrderBy(s => s.EndTime);
                    break;
                case "end_time_desc":
                    events = events.OrderByDescending(s => s.EndTime);
                    break;
                default:
                    events = events.OrderBy(s => s.EventName);
                    break;
            }

            //Used to get Parent and Foreign tables
            Event = await events.AsNoTracking().ToListAsync();
        }
    }
}

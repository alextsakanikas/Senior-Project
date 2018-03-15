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

        [TempData]
        public string StatusMessage { get; set; }

        public IList<Event> Event { get; set; }

        public string CurrentSort { get; set; }
        public string CurrentVenueSearch { get; set; }
        public string CurrentGenreSearch { get; set; }
        public string CurrentZipSearch { get; set; }

        #region Sorting Purposes
        public string EventNameSort { get; set; }
        public string VenueNameSort { get; set; }
        public string StartTimeSort { get; set; }
        public string EndTimeSort { get; set; }
        #endregion

        public async Task OnGetAsync(string sortOrder, string searchVenue, string searchGenre, string searchZip, string statusMessage)
        {
            EventNameSort = String.IsNullOrEmpty(sortOrder) ? "event_name_desc" : "";
            VenueNameSort = sortOrder == "Venue" ? "venue_desc" : "Venue";
            StartTimeSort = sortOrder == "Starttime" ? "start_time_desc" : "Starttime";
            EndTimeSort = sortOrder == "Endtime" ? "end_time_desc" : "Endtime";
            CurrentSort = sortOrder;

            StatusMessage = statusMessage;
            
            //Used to get Parent and Foreign tables as well as only showing future events
            IQueryable<Event> events = _context.Event.Include(ev => ev.Venue).ThenInclude(v => v.User).Include(ev => ev.Rsvps).Where(e => e.EndTime > DateTime.Now).AsQueryable();

            #region Filtering
            CurrentVenueSearch = searchVenue;
            CurrentGenreSearch = searchGenre;
            CurrentZipSearch = searchZip;
            if (!String.IsNullOrEmpty(searchVenue))
            {
                events = events.Where(s => s.Venue.VenueName.Contains(searchVenue));
            }
            if (!String.IsNullOrEmpty(searchGenre))
            {
                events = events.Where(s => s.Genre.Contains(searchGenre));
            }
            if (!String.IsNullOrEmpty(searchZip))
            {
                events = events.Where(s => s.Venue.Zip.ToString().Equals(searchZip));
            }
            #endregion

            switch (sortOrder)
            {
                case "event_name_desc":
                    events = events.OrderByDescending(s => s.EventName);
                    break;
                case "Venue":
                    events = events.OrderBy(s => s.Venue.VenueName);
                    break;
                case "venue_desc":
                    events = events.OrderByDescending(s => s.Venue.VenueName);
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

            Event = await events.AsNoTracking().ToListAsync();
        }
    }
}

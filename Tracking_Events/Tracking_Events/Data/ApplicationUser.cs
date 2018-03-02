using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Tracking_Events.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public int Zip { get; set; }

        public int AccountType { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Age Requirement")]
        public int AgeRequirement { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        //Used to reference AspNetUser table where the account is stored
        [ForeignKey("OwnerID")]
        public ApplicationUser User { get; set; }
    }
}

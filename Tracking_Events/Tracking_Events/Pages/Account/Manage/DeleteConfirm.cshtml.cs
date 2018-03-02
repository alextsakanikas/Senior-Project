using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class DeleteConfirmModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public DeleteConfirmModel(SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, ILogger<DeleteConfirmModel> logger)
        {
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }       

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync(string confirm)
        {
            if (confirm.Equals("yes"))
            {
                var user = _context.ApplicationUser.SingleOrDefault(a => a.UserName == User.Identity.Name);
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");

                _context.Remove(user);
                await _context.SaveChangesAsync();
                return LocalRedirect("/Index");
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QDEYE.Server.Data;
using QDEYE.Server.Migrations;
using QDEYE.Server.Models;


namespace QDEYE.Server.Areas.Identity.Pages.Account
{

    public class UserDetailsModel : PageModel
    {
        private readonly QDEYE.Server.Data.ApplicationDbContext _context;

        public UserDetailsModel(QDEYE.Server.Data.ApplicationDbContext context)
        {
            
            _context = context;
            
        }

        public IActionResult OnGet()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            return Page();
        }
     
        [BindProperty]
        public UserDetails UserDetails { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
           
            ApplicationUser user = _context.Users.Where(i => i.Email == HttpContext.User.Identity.Name).SingleOrDefault();
            UserDetails.UserId = user.Id;
            var details= _context.UserDetails.Where(i => i.UserId == UserDetails.UserId).SingleOrDefault();
            if (details != null)
            {
                details.IIN = UserDetails.IIN;
                details.SurName = UserDetails.SurName;
                details.Name = UserDetails.Name;
                details.MiddleName = UserDetails.MiddleName;
                _context.UserDetails.Attach(details);
                _context.Entry(details).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                _context.UserDetails.Add(UserDetails);
            }
          
            await _context.SaveChangesAsync();

            return RedirectToPage("");
        }
    }
}

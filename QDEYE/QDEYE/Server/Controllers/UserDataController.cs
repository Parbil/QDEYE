using QDEYE.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QDEYE.Server.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace QDEYE.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserDataController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly QDEYE.Server.Data.ApplicationDbContext _context;
        private readonly ILogger<UserDataController> logger;

        public UserDataController(QDEYE.Server.Data.ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private readonly IHttpContextAccessor _httpContextAccessor;
        [HttpPost]
        public async Task<bool> Post(UserDetailsModel userdata)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserDetails userDetails = new UserDetails();
            var details = new UserDetails();
            userDetails.UserId = userId;
            try
            {
                details = _context.UserDetails.Where(i => i.UserId == userDetails.UserId).SingleOrDefault();
            }
            catch(Exception ex) 
            { 
            
            }
            if (details!=null)
            {
                details.IIN = userdata.IIN;
                details.SurName = userdata.SurName;
                details.Name = userdata.Name;
                details.MiddleName = userdata.MiddleName;
                _context.UserDetails.Attach(details);
                _context.Entry(details).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                userDetails.IIN = userdata.IIN;
                userDetails.SurName = userdata.SurName;
                userDetails.Name = userdata.Name;
                userDetails.MiddleName = userdata.MiddleName;
                _context.UserDetails.Add(userDetails);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

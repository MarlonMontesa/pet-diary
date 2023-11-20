using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PetDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Pages.Home
{
    
    public class FriendModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public List<ApplicationUser> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }

        public FriendModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
            this.updates = _userManager.Users.ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
    }

    public class UserUpdates
    {
        public List<ApplicationUser> updates { get; set; }
    }
}

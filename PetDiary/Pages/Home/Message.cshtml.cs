using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Pages.Home
{
    [Authorize]
    public class MessageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        [BindProperty]
        public TextMsg messagess{ get; set; }
        
        [BindProperty]
        public List<Message> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }
        
        

        public MessageModel(ApplicationDbContext context, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            _userManger = userManger;
        }
        public void OnGet()
        {
            this.updates = _context.Messages.Include("user").Include(x => x.user).ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            
        }
        

        public async Task<IActionResult> OnPostMessageAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userMessage = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                Message message = new Message();
                message.Email = User.Identity.Name; 
                
                message.Text = messagess.Text;
                message.UserId = this.user.Id;
                message.user = userMessage;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                

                return Page();
            }
            return Page();
        }



        public class TextMsg
        {
            public Guid Id { get; set; }

            public string Email { get; set; }

            public string Text { get; set; }
            public DateTime Time { get; set; } = DateTime.Now;
           
            public Guid UserId { get; set; }
        }
    }
}

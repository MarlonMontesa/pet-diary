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

namespace PetDiary.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public PostData postData { get; set; }
        [BindProperty]
        public List<UserPost> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }
        [BindProperty]
        public UserComment userComment { get; set; }
        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
            this.updates = _context.UserPost.Include("user").Include("comments").Include(x => x.comments).ThenInclude(x => x.user).OrderByDescending(x => x.postCreated).ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userProfile = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                UserPost userPost = new UserPost();

                userPost.post = postData.post;
                userPost.status = postData.status;
                userPost.userId = this.user.Id;
                userPost.user = userProfile;

                await _context.UserPost.AddAsync(userPost);
                await _context.SaveChangesAsync();

                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            UserPost userPost = await _context.UserPost.FirstAsync(x => x.id == postData.id);
            userPost.post = postData.post;
            userPost.status = postData.status;
            _context.UserPost.Update(userPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        public IActionResult OnGetDelete(Guid id)
        {
            var userPost = _context.UserPost.Find(id);
            _context.Remove(userPost);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }


        public async Task<IActionResult> OnPostCommentAsync(Guid id)
        {
            if(!ModelState.IsValid) return Page();
            ApplicationUser commenter = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            UserPost userPost = await _context.UserPost.FirstOrDefaultAsync(x => x.id == id);
            Comments comments = new Comments
            {
                comment = userComment.comment,
                user = commenter,
                userPost = userPost
            };
            await _context.Comments.AddAsync(comments);
            var result = await _context.SaveChangesAsync();
            if (result == 1)
                return RedirectToPage("Index");
            return Page();



        }
    }
    public class PostData
    {
        public Guid id { get; set; }
        public string post { get; set; }
        public string status { get; set; }
        public DateTime postCreated { get; set; } = DateTime.Now;
    }

    public class PostUpdates
    {
        public List<UserPost> updates { get; set; }
    }
    public class UserComment
    {
        public string comment { get; set; }
    }
}

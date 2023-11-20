using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetDiary.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Pages.Home
{
    [Authorize]
    public class VideoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public VideoData videoData { get; set; }
        [BindProperty]
        public List<UserVideo> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }
        [BindProperty]

        public IFormFile uploadfiles { get; set; }

        public VideoModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }

        public void OnGet()
        {
            this.updates = _context.UserVideo.Include("user").Include(x => x.user).OrderByDescending(x => x.videoCreated).ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }

        public async Task<IActionResult> OnPostAsync(UserVideo userVideo)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userProfile = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                //UserVideo userVideo = new UserVideo();

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(userVideo.videoFile.FileName);
                string extension = Path.GetExtension(userVideo.videoFile.FileName);
                userVideo.videoName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/videos/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await userVideo.videoFile.CopyToAsync(fileStream);
                }


                userVideo.videoCaption = videoData.videoCaption;
                userVideo.userId = this.user.Id;
                userVideo.user = userProfile;

                await _context.UserVideo.AddAsync(userVideo);
                await _context.SaveChangesAsync();

                return RedirectToPage("Video");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            UserVideo userVideo = await _context.UserVideo.FirstAsync(x => x.id == videoData.id);

            userVideo.videoCaption = videoData.videoCaption;
            _context.UserVideo.Update(userVideo);
            await _context.SaveChangesAsync();
            return RedirectToPage("Video");
        }

        public IActionResult OnGetDelete(Guid id)
        {
            var userVideo = _context.UserVideo.Find(id);
            _context.Remove(userVideo);
            _context.SaveChanges();
            return RedirectToPage("Video");
        }
    }

    public class VideoData
    {
        public Guid id { get; set; }
        public string videoCaption { get; set; }
        public string videoName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile videoFile { get; set; }
        public DateTime videoCreated { get; set; } = DateTime.Now;
    }

    public class VideoUpdates
    {
        public List<UserVideo> updates { get; set; }
    }
}

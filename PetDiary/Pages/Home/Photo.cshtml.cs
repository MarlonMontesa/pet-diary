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
    public class PhotoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public PhotoData photoData { get; set; }
        [BindProperty]
        public List<UserPhoto> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }
        [BindProperty]

        public IFormFile uploadfiles { get; set; }

        public PhotoModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }

        public void OnGet()
        {
            this.updates = _context.UserPhoto.Include("user").Include(x => x.user).OrderByDescending(x => x.photoCreated).ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
        public async Task<IActionResult> OnPostAsync(UserPhoto userPhoto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userProfile = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                //UserPhoto userPhoto = new UserPhoto();

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(userPhoto.photoFile.FileName);
                string extension = Path.GetExtension(userPhoto.photoFile.FileName);
                userPhoto.photoName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await userPhoto.photoFile.CopyToAsync(fileStream);
                }


                userPhoto.photoCaption = photoData.photoCaption;
                userPhoto.userId = this.user.Id;
                userPhoto.user = userProfile;

                await _context.UserPhoto.AddAsync(userPhoto);
                await _context.SaveChangesAsync();

                return RedirectToPage("Photo");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            UserPhoto userPhoto = await _context.UserPhoto.FirstAsync(x => x.id == photoData.id);

            userPhoto.photoCaption = photoData.photoCaption;
            _context.UserPhoto.Update(userPhoto);
            await _context.SaveChangesAsync();
            return RedirectToPage("Photo");
        }

        public IActionResult OnGetDelete(Guid id)
        {
            var userPhoto = _context.UserPhoto.Find(id);
            _context.Remove(userPhoto);
            _context.SaveChanges();
            return RedirectToPage("Photo");
        }
    }
    public class PhotoData
    {
        public Guid id { get; set; }
        public string photoCaption { get; set; }
        public string photoName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile photoFile { get; set; }
        public DateTime photoCreated { get; set; } = DateTime.Now;
    }

    public class PhotoUpdates
    {
        public List<UserPhoto> updates { get; set; }
    }
}

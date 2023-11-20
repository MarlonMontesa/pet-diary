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
    public class AdoptionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public AdoptionData adoptionData { get; set; }
        [BindProperty]
        public List<UserAdoption> updates { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }
        [BindProperty]
        public IFormFile uploadfiles { get; set; }
        [BindProperty]
        public UserComment userComment { get; set; }

        public AdoptionModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }

        public void OnGet()
        {
            this.updates = _context.UserAdoption.Include("user").Include("adoptComments").Include(x => x.adoptComments).ThenInclude(x => x.user).OrderByDescending(x => x.adoptCreated).ToList();
            this.user = _context.users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
        public async Task<IActionResult> OnPostAsync(UserAdoption userAdoption)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userProfile = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                //UserAdoption userAdoption = new UserAdoption();

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(userAdoption.adoptFile.FileName);
                string extension = Path.GetExtension(userAdoption.adoptFile.FileName);
                userAdoption.adoptName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/adoption/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await userAdoption.adoptFile.CopyToAsync(fileStream);
                }


                userAdoption.adoptCaption = adoptionData.adoptCaption;
                userAdoption.userId = this.user.Id;
                userAdoption.user = userProfile;

                await _context.UserAdoption.AddAsync(userAdoption);
                await _context.SaveChangesAsync();

                return RedirectToPage("Adoption");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            UserAdoption userAdoption = await _context.UserAdoption.FirstAsync(x => x.id == adoptionData.id);

            userAdoption.adoptCaption = adoptionData.adoptCaption;
            _context.UserAdoption.Update(userAdoption);
            await _context.SaveChangesAsync();
            return RedirectToPage("Adoption");
        }
        public IActionResult OnGetDelete(Guid id)
        {
            var userAdoption = _context.UserAdoption.Find(id);
            _context.Remove(userAdoption);
            _context.SaveChanges();
            return RedirectToPage("Adoption");
        }

        public async Task<IActionResult> OnPostCommentAsync(Guid id)
        {
            if (!ModelState.IsValid) return Page();
            ApplicationUser commenter = await _context.users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            UserAdoption userAdoption = await _context.UserAdoption.FirstOrDefaultAsync(x => x.id == id);
            AdoptComments adoptComments = new AdoptComments
            {
                comment = userComment.comment,
                user = commenter,
                userAdoption = userAdoption
            };
            await _context.adoptComments.AddAsync(adoptComments);
            var result = await _context.SaveChangesAsync();
            if (result == 1)
                return RedirectToPage("Adoption");
            return Page();



        }
    }

    public class AdoptionData
    {
        public Guid id { get; set; }
        public string adoptCaption { get; set; }
        public string adoptName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile adoptFile { get; set; }
        public DateTime adoptCreated { get; set; } = DateTime.Now;
    }

    public class AdoptionUpdates
    {
        public List<UserAdoption> updates { get; set; }
    }

    public class UserComment
    {
        public string comment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetDiary.Model;

namespace PetDiary.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public FormData formdata { get; set; }
        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {
        }
        public async Task <IActionResult> OnPostAsync(ApplicationUser user)
        {
            
            if (ModelState.IsValid)
            {
                var checkuser = await _userManager.FindByNameAsync(formdata.UserName);
                var checkemail = await _userManager.FindByEmailAsync(formdata.Email);

                if (checkuser == null && checkemail == null)
                {
                    //ApplicationUser user = new ApplicationUser();

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(user.PictureFile.FileName);
                    string extension = Path.GetExtension(user.PictureFile.FileName);
                    user.ProfilePicture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/profile/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.PictureFile.CopyToAsync(fileStream);
                    }

                    user.FirstName = formdata.FirstName;
                    user.LastName = formdata.LastName;
                    user.UserName = formdata.UserName;
                    user.Email = formdata.Email;

                    var create = await _userManager.CreateAsync(user, formdata.password);
                    if (create.Succeeded) 
                    {
                        var login = await _signInManager.PasswordSignInAsync(formdata.UserName, formdata.password, false, false);

                        if (login.Succeeded)
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                    foreach (var error in create.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Page();
        }
    }

    public class FormData
    {
        public string ProfilePicture { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile PictureFile { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}


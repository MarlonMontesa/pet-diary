using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public partial class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public string ProfilePicture { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile PictureFile { get; set; }
        public List<UserPost> userPosts { get; set; }
        public List<Comments> comments { get; set; }
        public List<UserPhoto> userPhotos { get; set; }
        public List<UserVideo> userVideos { get; set; }
        public List<UserAdoption> userAdoptions { get; set; }
        public List<AdoptComments> adoptComments { get; set; }
        public List<Message> messages { get; set; }


    }
}

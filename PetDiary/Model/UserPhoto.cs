using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class UserPhoto
    {
        public Guid id { get; set; }
        public string photoCaption { get; set; }
        public string photoName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile photoFile { get; set; }
        public DateTime photoCreated { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
    }
}

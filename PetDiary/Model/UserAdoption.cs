using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class UserAdoption
    {
        public Guid id { get; set; }
        public string adoptCaption { get; set; }
        public string adoptName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile adoptFile { get; set; }
        public DateTime adoptCreated { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
        public List<AdoptComments> adoptComments { get; set; }

    }
}

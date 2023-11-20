using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class UserPost
    {
        public Guid id { get; set; }
        public string post { get; set; }
        public string status { get; set; }
        public DateTime postCreated { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
        public List<Comments> comments { get; set; }
    }
}

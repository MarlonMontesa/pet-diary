using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class AdoptComments
    {
        public Guid id { get; set; }
        public string comment { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
        public UserAdoption userAdoption { get; set; }
        public Guid userAdoptionId { get; set; }
    }
}

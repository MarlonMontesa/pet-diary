using System;

namespace PetDiary.Model
{
    public class Comments
    {
        public Guid id { get; set; }
        public string comment { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
        public UserPost userPost { get; set; }
        public Guid userPostId { get; set; }
    }
}

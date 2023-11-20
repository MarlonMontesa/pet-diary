using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid UserId { get; set; }
        
    }
}

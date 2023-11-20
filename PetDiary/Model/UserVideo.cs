﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public class UserVideo
    {
        public Guid id { get; set; }
        public string videoCaption { get; set; }
        public string videoName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile videoFile { get; set; }
        public DateTime videoCreated { get; set; } = DateTime.Now;
        public ApplicationUser user { get; set; }
        public Guid userId { get; set; }
    }
}

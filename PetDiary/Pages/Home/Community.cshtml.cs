using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Pages.Home
{
    
    public class CommunityModel : PageModel
    {
        private readonly ILogger<CommunityModel> _logger;
        

        public CommunityModel(ILogger<CommunityModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}

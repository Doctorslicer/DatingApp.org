using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(2)]
        public string username { get; set; }

        [Required]
        [MinLength(2)]
        public string password { get; set; }
    }
}
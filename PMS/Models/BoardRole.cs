﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class BoardRole
    {
        public int BoardRoleId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter alphabets only, please !")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters.")]
        public string BoardRoleName { get; set; }
    }
}

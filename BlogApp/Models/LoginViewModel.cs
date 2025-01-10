﻿using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage ="{0} alanı en az {2} karakter olabilir", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}

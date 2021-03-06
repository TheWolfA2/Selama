﻿using System.ComponentModel.DataAnnotations;

namespace Selama.Areas.Account.ViewModels.Manage
{
    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
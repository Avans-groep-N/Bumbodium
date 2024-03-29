﻿using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models.Utilities.ListValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [AllowNull]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public virtual string? ConfirmPassword { get; set; }

        [ListHasItems(ErrorMessage = "Selecteer ten minste 1 afdeling")]
        public List<int> Departments { get; set; } = new();
    }
}

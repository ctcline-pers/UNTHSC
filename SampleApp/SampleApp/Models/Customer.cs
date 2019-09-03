using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SampleApp.Models
{
    public class Customer
    {
        public int Id {get; set;}

        [Display(Name="First")]
        [MaxLength(256)]
        [Required]
        public string FirstName {get; set;}

        [Display(Name="Last")]
        [MaxLength(256)]
        [Required]
        public string LastName {get; set;}

        [MaxLength(256)]
        [Required]
        public string Email {get; set;}

        [Display(Name="DOB")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth {get; set;}

        [Display(Name="Zip Code")]
        [MaxLength(10)]
        [Required]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$")]
        public string ZipCode {get; set;}

        [MaxLength(256)]
        [Required]
        public string Country {get; set;}

        [Display(Name="System Role")]
        [MaxLength(256)]
        [Required]
        public string SystemRole {get; set;}

        [Display(Name="Created Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate {get; set;}
    }
}
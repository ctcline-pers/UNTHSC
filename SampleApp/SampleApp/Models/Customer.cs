using System;
using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models
{
    public class Customer
    {
        public int CustomerId {get; set;}
        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string Email {get; set;}

        [DataType(DataType.Date)]
        public DateTime DateOfBirth {get; set;}

        public string ZipCode {get; set;}

        public string Country {get; set;}

        public string SystemRole {get; set;}
    }
}
using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsDeleted { get; set; }

        public Users()
        {

        }

        public Users(string firstName, string lastName, bool? gender, string password, string emailAddress, string phoneNumber, bool? isVerified, bool? isDeleted)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Password = password;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            IsVerified = isVerified;
            IsDeleted = isDeleted;
        }
    }
}

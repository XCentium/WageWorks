using System;

namespace Wageworks.Foundation.Analytics.Models
{
    public class Contact
    {
        public string Identifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
    }
}
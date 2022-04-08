using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }     
        public string Bio { get; set; } 
        public ICollection<ActivityAttendee> Activities { get; set; } // ActivityAttendee is an 'associative' entity
        public ICollection<Photo> Photos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    // join entity (we have self-referencing many-to-many relationship between Users)
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        public AppUser Observer { get; set; } // follower
        public string TargetId { get; set; }
        public AppUser Target { get; set; } // following
    }
}
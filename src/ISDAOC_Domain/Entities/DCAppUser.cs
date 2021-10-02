using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppUser : IdentityUser
    {
        public DCAppUser()
        {
        }

        public DCAppUser(string username) : base(username)
        {
        }

        public string SortName { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsInternal { get; set; }

    }
}
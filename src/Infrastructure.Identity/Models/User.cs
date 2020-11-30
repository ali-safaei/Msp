using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity.Models
{
    public class User : IdentityUser,IBaseEntity
    {
        public DateTime LastLoginDateUtc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsSuperAdministrator { get; set; }
        public bool IsActive { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Chating.Models

{
    public class ApplicationUser:IdentityUser // to add new attribute .....
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } 

        [Required, MaxLength(100)]
        public string LastName { get; set; }

       // public byte[] ProfilePicture { get; set; } // to save image data in data base ...

        internal void OnModelCreating(ModelBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}

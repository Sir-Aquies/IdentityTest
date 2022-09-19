#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityTest.Models
{
    public class IdenTestUser : IdentityUser
    {
        [BindNever]
        public ICollection<Post> Posts { get; set; }
    }
}

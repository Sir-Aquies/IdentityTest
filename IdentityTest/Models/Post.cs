#nullable disable
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string PostContent { get; set; }
		public string Media { get; set; }
		public DateTime CreatedDate { get; set; }

		public IdenTestUser User { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}

#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Models
{
	public class Comments
	{
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        [Required]
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }

        public IdenTestUser User { get; set; }
        public Post Post { get; set; }
        //public ICollection<ReplyModel> Replies { get; set; }
    }
}

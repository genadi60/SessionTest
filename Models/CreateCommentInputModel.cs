using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SessionTest.MappingServices;

namespace SessionTest.Models
{
    public class CreateCommentInputModel: IMapFrom<Comment>, IMapTo<CommentViewModel>, IMapTo<Comment>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Content { get; set; }
        
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}

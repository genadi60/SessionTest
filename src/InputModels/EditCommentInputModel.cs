using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.InputModels
{
    public class EditCommentInputModel: IMapFrom<Comment>, IMapTo<CommentViewModel>, IMapTo<Comment>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Content { get; set; }
        
        [Required]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [Required]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

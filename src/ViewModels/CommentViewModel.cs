using Microsoft.AspNetCore.Identity;
using SessionTest.InputModels;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class CommentViewModel : IMapFrom<Comment>, IMapTo<CreateCommentInputModel>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

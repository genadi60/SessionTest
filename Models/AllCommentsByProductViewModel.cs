using System.Collections.Generic;

namespace SessionTest.Models
{
    public class AllCommentsByProductViewModel
    {
        public AllCommentsByProductViewModel()
        {
            CommentViewModels = new HashSet<CommentViewModel>();
        }

        public IEnumerable<CommentViewModel> CommentViewModels { get; set; }
    }
}

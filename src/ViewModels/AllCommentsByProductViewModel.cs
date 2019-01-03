using System.Collections.Generic;

namespace SessionTest.ViewModels
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

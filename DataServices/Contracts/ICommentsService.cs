using SessionTest.Models;
using System.Threading.Tasks;

namespace SessionTest.DataServices.Contracts
{
    public interface ICommentsService
    {
         AllCommentsByProductViewModel GetAllByProduct(string id);

        Task<string> Add(CreateCommentInputModel model);

        Task<string> Edit(CommentViewModel model);

        Task<int> Delete(string id);

        CommentViewModel GetById(string id);
    }
}

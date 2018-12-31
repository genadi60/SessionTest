using SessionTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionTest.DataServices.Contracts
{
    public interface IBlogsService
    {
        IEnumerable<BlogViewModel> GetAll();

        Task<string> Create(string title, string content);

        TViewModel GetBlogById<TViewModel>(string id);

        //bool IsCategoryIdValid(int categoryId);

        //int GetCategoryId(string name);

        //IEnumerable<BlogViewModel> GetAllPostsFromCategory(int categoryId);
    }
}

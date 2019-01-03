using SessionTest.DataServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Common;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.DataServices
{
    public class BlogsService : IBlogsService
    {
        private readonly IRepository<Blog> blogsRepository;

        public BlogsService(IRepository<Blog> blogsRepository)
        {
            this.blogsRepository = blogsRepository;
        }

        public IEnumerable<BlogViewModel> GetAll() => this.blogsRepository.All().To<BlogViewModel>();

        public async Task<string> Create(string title, string content)
        {
            var blog = new Blog
            {
                Title = title,
                Content = content,
                PostedOn = DateTime.UtcNow
            };

            await this.blogsRepository.AddAsync(blog);
            await this.blogsRepository.SaveChangesAsync();

            return blog.Id;
        }

        public TViewModel GetBlogById<TViewModel>(string id)
        {
            var blog = blogsRepository.All().Where(x => x.Id == id).To<TViewModel>().FirstOrDefault();

            return blog;
        }
    }
}

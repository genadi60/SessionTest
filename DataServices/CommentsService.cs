using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.DataServices
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> _commentsRepository;

        public CommentsService(IRepository<Comment> commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public AllCommentsByProductViewModel GetAllByProduct(string id)
        {
            var comments = _commentsRepository.All()
                .Where(c => c.ProductId == id)
                .To<CommentViewModel>()
                //.Select(c => new CommentViewModel
                //{
                //    Id = c.Id,
                //    Content = c.Content,
                //    ProductId = c.ProductId,
                //    UserId = c.UserId
                //})
                .ToList();

            var model = new AllCommentsByProductViewModel
            {
                CommentViewModels = comments
            };

            return model;
        }

        public async Task<string> Add(CreateCommentInputModel model)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = model.Content,
                ProductId = model.ProductId,
                UserId = model.UserId
            };

            await _commentsRepository.AddAsync(comment);
            await _commentsRepository.SaveChangesAsync();

            var id = comment.Id;/*_commentsRepository.All().Single(c => c.Content.Equals(model.Content)).Id;*/

            return id;
        }

        public async Task<string> Edit(CommentViewModel model)
        {
            var comment = _commentsRepository.All()
                .FirstOrDefault(x => x.Id == model.Id); ;

            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            comment.Content = model.Content;

            _commentsRepository.Update(comment);
            await _commentsRepository.SaveChangesAsync();

            var id = comment.Id;

            return id;
        }

        public async Task<int> Delete(string id)
        {
            var comment = this._commentsRepository.All()
                .FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            _commentsRepository.Delete(comment);

            var index = await _commentsRepository.SaveChangesAsync();

            return index;
        }

        public CommentViewModel GetById(string id)
        {
            var model = _commentsRepository.All()
                .Where(c => c.Id == id)
                .To<CommentViewModel>()
                .FirstOrDefault();

            return model;  
        }
    }
}

using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {

        private readonly IUnitOfWork _unitOfWork;
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.PostRepository.GetAll();
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new Exception("User does not exist!");
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed!");
            }

            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPosts.Count() > 0 && userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(p => p.Date).FirstOrDefault();
                if (lastPost != null && (DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new Exception("New users cannot create more than one post per week.");
                }
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var originalPost = await GetPost(post.Id);
            originalPost.Description = post.Description;
            originalPost.Date = post.Date;
            originalPost.Image = post.Image;

            _unitOfWork.PostRepository.Update(originalPost);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

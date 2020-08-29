using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
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

using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
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

        public async Task<PagedList<Post>> GetPosts(PostQueryFilter filters)
        {
            var posts = await _unitOfWork.PostRepository.GetAll();

            if (filters.UserId != null)
            {
                posts = posts.Where(p => p.UserId == filters.UserId);
            }

            if (filters.Date != null)
            {
                posts = posts.Where(p => p.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }

            if (filters.Description != null)
            {
                posts = posts.Where(p => p.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var PagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.pageSize);

            return PagedPosts;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessException("User does not exist!");
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Content not allowed!");
            }

            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPosts.Count() > 0 && userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(p => p.Date).FirstOrDefault();
                if (lastPost != null && (DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("New users cannot create more than one post per week.");
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

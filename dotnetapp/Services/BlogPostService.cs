using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Data;
using dotnetapp.Exceptions;
using dotnetapp.Models;
using Microsoft.AspNetCore.Authorization;
 
namespace dotnetapp.Services
{
    public class BlogPostService
    {
        public readonly ApplicationDbContext _dbContext;
 
        public BlogPostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

      
        public async Task<IEnumerable<BlogPost>> GetAllBlogPosts()
        {
             return await _dbContext.BlogPosts
                .Include(bp => bp.User)
                .ToListAsync();
        }
 

        public async Task<BlogPost> GetBlogPostById(int blogPostId)
        {
            return await _dbContext.BlogPosts
                .Include(bp => bp.User)
                .FirstOrDefaultAsync(bp => bp.BlogPostId == blogPostId);
        }
 
        public async Task<IEnumerable<BlogPost>> GetBlogPostsByUserId(int userId)
        {
             return await _dbContext.BlogPosts
                 .Where(bp => bp.UserId == userId)
                 .ToListAsync();
        }
 
        public async Task<bool> AddBlogPost(BlogPost blogPost)
        {
            if (await _dbContext.BlogPosts.AnyAsync(bp => bp.Title == blogPost.Title))
            {
                throw new BlogPostException("A blog post with the same title already exists");
            }
 
            _dbContext.BlogPosts.Add(blogPost);
            await _dbContext.SaveChangesAsync();
             return true;
        }
 
        public async Task<bool> UpdateBlogPost(int blogPostId, BlogPost blogPost)
        {
            var existingBlogPost = await GetBlogPostById(blogPostId);
            if (existingBlogPost == null)
            {
                return false;
            }
 
            existingBlogPost.UserId = blogPost.UserId;
            existingBlogPost.Title = blogPost.Title;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.Status = blogPost.Status;
            existingBlogPost.PublishedDate = blogPost.PublishedDate;
           
            await _dbContext.SaveChangesAsync();
             return true;
        }
 
        public async Task<bool> DeleteBlogPost(int blogPostId)
        {
            var blogPost = await GetBlogPostById(blogPostId);
            if (blogPost == null)
            {
                return false;
            }
 
            _dbContext.BlogPosts.Remove(blogPost);
            await _dbContext.SaveChangesAsync();
             return true;
        }
    }
}
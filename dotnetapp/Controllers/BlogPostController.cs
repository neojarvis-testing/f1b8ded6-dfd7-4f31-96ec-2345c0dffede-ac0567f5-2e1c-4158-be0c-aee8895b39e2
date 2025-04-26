using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using dotnetapp.Models;
using dotnetapp.Services;
using dotnetapp.Exceptions;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly BlogPostService _blogPostService;
 
        public BlogPostController(BlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }
 
        [HttpGet]
        [Authorize(Roles = "Admin, User")] 
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostService.GetAllBlogPosts();
            return Ok(blogPosts);
        }
 
        [HttpGet("{postId}")]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult<BlogPost>> GetBlogPostById(int postId)
        {
            var blogPost = await _blogPostService.GetBlogPostById(postId);
            if (blogPost == null)
            {
                return NotFound("Blog post not found");
            }
            return Ok(blogPost);
        }
 
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPostsByUserId(int userId)
        {
            var blogPosts = await _blogPostService.GetBlogPostsByUserId(userId);
            if (blogPosts == null || blogPosts.Count() == 0)
            {
                return NotFound("No blog posts found for this user");
            }
            return Ok(blogPosts);
        }
 
        [HttpPost]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult> AddBlogPost([FromBody] BlogPost blogPost)
        {
            try
            {
                await _blogPostService.AddBlogPost(blogPost);
                return Ok("Blog post added successfully");
            }
            catch (BlogPostException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
 
        [HttpPut("{postId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateBlogPost(int postId, [FromBody] BlogPost blogPost)
        {
            try
            {
                var success = await _blogPostService.UpdateBlogPost(postId, blogPost);
                if (!success)
                {
                    return NotFound("Blog post not found");
                }
                return Ok("Blog post updated successfully");
            }
            catch (BlogPostException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
 
        [HttpDelete("{postId}")]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult> DeleteBlogPost(int postId)
        {
            try
            {
                var success = await _blogPostService.DeleteBlogPost(postId);
                if (!success)
                {
                    return NotFound("Blog post not found");
                }
                return Ok("Blog post deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

    }
}
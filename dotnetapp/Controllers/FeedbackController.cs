using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
 
        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
 
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacks();
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
 
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(int userId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserId(userId);
                if (feedbacks == null || !feedbacks.Any())
                {
                    return NotFound("No feedbacks found for this user");
                }
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
 
        [HttpPost]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            try
            {
                await _feedbackService.AddFeedback(feedback);
                return Ok("Feedback added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
 
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")] 
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedback(id);
                if (!result)
                {
                    return NotFound("Cannot find any feedback");
                }
                return Ok("Feedback deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
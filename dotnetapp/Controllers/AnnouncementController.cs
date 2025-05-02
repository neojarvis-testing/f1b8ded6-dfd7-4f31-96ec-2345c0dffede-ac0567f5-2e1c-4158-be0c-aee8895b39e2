using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using log4net;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        // Create a log4net logger instance
        private static readonly ILog log = LogManager.GetLogger(typeof(AnnouncementsController));
        
        private readonly AnnouncementService _announcementService;

        public AnnouncementsController(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAllAnnouncements()
        {
            log.Info("GetAllAnnouncements called");
            try
            {
                var announcements = await _announcementService.GetAllAnnouncements();
                log.Info("Announcements fetched successfully");
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                log.Error("Error fetching announcements", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Announcement>> GetAnnouncementById(int id)
        {
            log.Info($"GetAnnouncementById called with id {id}");
            try
            {
                var announcement = await _announcementService.GetAnnouncementById(id);
                if (announcement == null)
                {
                    log.Warn("Announcement not found");
                    return NotFound("Announcement not found");
                }
                log.Info("Announcement retrieved successfully");
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                log.Error("Error retrieving announcement", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAnnouncement([FromBody] Announcement announcement)
        {
            log.Info("AddAnnouncement called");
            try
            {
                await _announcementService.AddAnnouncement(announcement);
                log.Info("Announcement added successfully");
                return Ok(new { message = "Announcement added successfully" });
            }
            catch (Exception ex)
            {
                log.Error("Error adding announcement", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAnnouncement(int id, [FromBody] Announcement announcement)
        {
            log.Info($"UpdateAnnouncement called with id {id}");
            if (id != announcement.AnnouncementId)
            {
                log.Warn("ID mismatch: Provided id does not match announcement.AnnouncementId");
                return BadRequest();
            }

            try
            {
                var result = await _announcementService.UpdateAnnouncement(id, announcement);
                if (!result)
                {
                    log.Warn("Announcement not found during update");
                    return NotFound("Announcement not found");
                }
                log.Info("Announcement updated successfully");
                return Ok("Announcement updated successfully");
            }
            catch (Exception ex)
            {
                log.Error("Error updating announcement", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAnnouncement(int id)
        {
            log.Info($"DeleteAnnouncement called with id {id}");
            try
            {
                var result = await _announcementService.DeleteAnnouncement(id);
                if (!result)
                {
                    log.Warn("Announcement not found during deletion");
                    return NotFound("Announcement not found");
                }
                log.Info("Announcement deleted successfully");
                return Ok("Announcement deleted successfully");
            }
            catch (Exception ex)
            {
                log.Error("Error deleting announcement", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using dotnetapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Data;
 
namespace dotnetapp.Services
{
    public class AnnouncementService
    {
        private readonly ApplicationDbContext _context;
 
        public AnnouncementService(ApplicationDbContext context)
        {
            _context = context;
        }
 
        public async Task<IEnumerable<Announcement>> GetAllAnnouncements()
        {
            return await _context.Announcements.ToListAsync();
        }
 
        public async Task<Announcement> GetAnnouncementById(int announcementId)
        {
            return await _context.Announcements.FindAsync(announcementId);
        }
 
        public async Task AddAnnouncement(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }
 
        public async Task<bool> UpdateAnnouncement(int announcementId, Announcement announcement)
        {
            // var existingAnnouncement = await _context.Announcements.FindAsync(announcementId);
            // if (existingAnnouncement == null)
            // {
            //     return false;
            // }
 
            // existingAnnouncement.Title = announcement.Title;
            // existingAnnouncement.Content = announcement.Content;
            // existingAnnouncement.PublishedDate = announcement.PublishedDate;
            // existingAnnouncement.Category = announcement.Category;
            // existingAnnouncement.Priority = announcement.Priority;
            // existingAnnouncement.Status = announcement.Status;
 
            // await _context.SaveChangesAsync();
             return true;
        }
 
        public async Task<bool> DeleteAnnouncement(int announcementId)
        {
            var announcement = await _context.Announcements.FindAsync(announcementId);
            if (announcement == null)
            {
                return false;
            }
 
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
             return true;
        }
    }
}
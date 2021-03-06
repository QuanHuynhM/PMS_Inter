﻿using PMS.Models;
using PMS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Persistence.IRepository
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> GetAnnouncement(int? id, bool includeRelated = true);
        void AddAnnouncement(Announcement Announcement);
        void RemoveAnnouncement(Announcement Announcement);
        Task<QueryResult<Announcement>> GetAnnouncements(Query filter);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        void UpdateAnnouncementUsers(Announcement announcement, AnnouncementResource announcementResource);
    }
}

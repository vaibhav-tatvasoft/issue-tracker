﻿
namespace ChattingApplication.DataAccess.Repository
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Group = ChattingApplication.Models.Group;

    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDBContext _db;

        public GroupRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        // Get a group by its ID
        public async Task<Group?> GetGroupAsync(Expression<Func<Group, bool>> filter)
        {
            var query = _db.Set<Group>();
            return query.Where(filter).FirstOrDefault();
        }

        // Get all groups
        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return _db.Groups.ToList();
        }

        // Create a new group
        public async Task<Group> CreateGroupAsync(Group group)
        {
            try
            {
                _db.Groups.Add(group);
                await _db.SaveChangesAsync();
                return group;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        // Update an existing group
        public async Task UpdateGroupAsync(Group group)
        {
            var existingGroup = _db.Groups.FirstOrDefault(g => g.id == group.id);
            if (existingGroup != null)
            {
                existingGroup.groupName = group.groupName;
                existingGroup.groupId = group.groupId;
                //existingGroup.membersIds = group.membersIds;
                existingGroup.messages = group.messages;
                existingGroup.createdBy = group.createdBy;
                existingGroup.lastMessage = group.lastMessage;
                existingGroup.lastMessageTimestamp = group.lastMessageTimestamp;
                //existingGroup.isTyping = group.isTyping;
                existingGroup.groupAvatar = group.groupAvatar;
            }
            _db.SaveChanges();
            await Task.CompletedTask;
        }

        // Delete a group
        public async Task DeleteGroupAsync(string id)
        {
            var groupToRemove = _db.Groups.FirstOrDefault(g => g.id == id);
            if (groupToRemove != null)
            {
                _db.Remove(groupToRemove);
                _db.SaveChanges();
            }
            await Task.CompletedTask;
        }
    }

    public interface IGroupRepository
    {
        Task<Group?> GetGroupAsync(Expression<Func<Group, bool>> filter);
        Task<List<Group>> GetAllGroupsAsync();
        Task<Group> CreateGroupAsync(Group group);
        Task UpdateGroupAsync(Group group);
        Task DeleteGroupAsync(string id);
    }
}

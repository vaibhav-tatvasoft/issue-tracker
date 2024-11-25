
namespace ChattingApplication.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Group = ChattingApplication.Models.Group;

    public class GroupRepository : IGroupRepository
    {
        private readonly List<Models.Group> _groups = new List<Group>();

        // Get a group by its ID
        public async Task<Group?> GetGroupByIdAsync(string id)
        {
            return await Task.FromResult(_groups.FirstOrDefault(g => g.id == id));
        }

        // Get all groups
        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await Task.FromResult(_groups);
        }

        // Create a new group
        public async Task CreateGroupAsync(string groupName)
        {
            Group group = new Group();
            // Assuming that GroupId is assigned by the client or elsewhere, otherwise generate it here
            group.id = groupName;
            group.createdAt = DateTime.Now;
            _groups.Add(group);
            await Task.CompletedTask;
        }

        // Update an existing group
        public async Task UpdateGroupAsync(Group group)
        {
            var existingGroup = _groups.FirstOrDefault(g => g.id == group.id);
            if (existingGroup != null)
            {
                existingGroup.groupName = group.groupName;
                existingGroup.groupDescription = group.groupDescription;
                existingGroup.members = group.members;
                existingGroup.messages = group.messages;
                existingGroup.createdBy = group.createdBy;
                existingGroup.lastMessage = group.lastMessage;
                existingGroup.lastMessageTimestamp = group.lastMessageTimestamp;
                existingGroup.isTyping = group.isTyping;
                existingGroup.groupAvatar = group.groupAvatar;
            }
            await Task.CompletedTask;
        }

        // Delete a group
        public async Task DeleteGroupAsync(string id)
        {
            var groupToRemove = _groups.FirstOrDefault(g => g.id == id);
            if (groupToRemove != null)
            {
                _groups.Remove(groupToRemove);
            }
            await Task.CompletedTask;
        }
    }

    public interface IGroupRepository
    {
        Task<Group?> GetGroupByIdAsync(string id);
        Task<List<Group>> GetAllGroupsAsync();
        Task CreateGroupAsync(string groupName);
        Task UpdateGroupAsync(Group group);
        Task DeleteGroupAsync(string id);
    }
}

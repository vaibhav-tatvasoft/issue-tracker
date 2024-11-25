using ChattingApplication.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Group = ChattingApplication.Models.Group;

public class GroupService
{
    private readonly IGroupRepository _groupRepository;
     
    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group?> GetGroupByIdAsync(string groupId)
    {
        return await _groupRepository.GetGroupByIdAsync(groupId);
    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await _groupRepository.GetAllGroupsAsync();
    }

    public async Task CreateGroupAsync(string groupName)
    {
        // Add any validation or logic here before creating
        await _groupRepository.CreateGroupAsync(groupName);
    }

    public async Task UpdateGroupAsync(Group group)
    {
        // Add any validation or logic here before updating
        await _groupRepository.UpdateGroupAsync(group);
    }

    public async Task DeleteGroupAsync(string groupId)
    {
        await _groupRepository.DeleteGroupAsync(groupId);
    }
}

using ChattingApplication.DataAccess;
using ChattingApplication.DataAccess.Repository;
using ChattingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Threading.Tasks;
using Group = ChattingApplication.Models.Group;

public class GroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly ApplicationDBContext _dbContext;

    public GroupService(IGroupRepository groupRepository, ApplicationDBContext dbContext)
    {
        _groupRepository = groupRepository;
        _dbContext = dbContext;
    }

    public async Task<Group?> GetGroupByIdAsync(string groupId)
    {
        return await _groupRepository.GetGroupByIdAsync(groupId);
    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await _groupRepository.GetAllGroupsAsync();
    }

    public async Task<Group> CreateGroupAsync(User fromUserObject, string id, List<User> members, string groupName)
    {
        var group = new Group
        {
            id = id,
            groupName = groupName,
            members = members,
            createdBy = fromUserObject.id,
            createdAt = DateTime.Now
        };

        try
        {
            var response = await _groupRepository.CreateGroupAsync(group);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
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

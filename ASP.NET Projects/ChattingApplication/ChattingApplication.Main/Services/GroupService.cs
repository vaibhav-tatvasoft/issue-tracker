using ChattingApplication.DataAccess;
using ChattingApplication.DataAccess.Repository;
using ChattingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Group = ChattingApplication.Models.Group;

public class GroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly ApplicationDBContext _dbContext;
    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, ApplicationDBContext dbContext, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<Group?> GetGroupAsync(Expression<Func<Group, bool>> filter)
    {

        return await _groupRepository.GetGroupAsync(filter);
    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await _groupRepository.GetAllGroupsAsync();
    }

    public async Task<Group> CreateGroupAsync(User fromUserObject, string id, List<string> members, string groupName)
    {
        var group = new Group
        {
            id = Guid.NewGuid().ToString(),
            groupId = id,
            groupName = groupName,
            //membersIds = members,
            createdBy = fromUserObject.id,
            createdAt = DateTime.Now
        };

        try
        {
            var response = await _groupRepository.CreateGroupAsync(group);
            var userObj = _userRepository.GetAllUsers(u => members.Contains(u.id)).ToList();

            foreach(var user in userObj)
            {
                //UserGroup gr
                //user.groups.Add(group);
                _userRepository.UpdateUser(user);
            }
                _dbContext.SaveChanges();

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

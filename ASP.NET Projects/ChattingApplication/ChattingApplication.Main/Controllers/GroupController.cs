using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Group = ChattingApplication.Models.Group;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService)
    {
        _groupService = groupService;
    }

    // GET api/group/{id}
    //[HttpGet("{id}")]
    //public async Task<ActionResult<Group>> GetGroupById(string id)
    //{
    //    var group = await _groupService.GetGroupAsync(id);
    //    if (group == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(group);
    //}

    // GET api/group
    [HttpGet]
    public async Task<ActionResult<List<Group>>> GetAllGroups()
    {
        var groups = await _groupService.GetAllGroupsAsync();
        return Ok(groups);
    }

    // POST api/group
    [HttpPost]
    public async Task<ActionResult> CreateGroup([FromBody] string groupName)
    {
        await _groupService.CreateGroupAsync(null,Guid.NewGuid().ToString(), null, groupName);
        return Ok();
    }

    // PUT api/group/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGroup(string id, [FromBody] Group group)
    {
        if (id != group.id)
        {
            return BadRequest();
        }
        await _groupService.UpdateGroupAsync(group);
        return NoContent();
    }

    // DELETE api/group/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(string id)
    {
        await _groupService.DeleteGroupAsync(id);
        return NoContent();
    }
}

using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Services;

public class GroupService(IRepository<Group> groupRepository)
{
    public async Task AddOrUpdateGroup(Group group)
    {
        await groupRepository.AddOrUpdateAsync(group);
    }

    public async Task<List<GroupDto>> GetGroups()
    {
        var courses = await groupRepository.GetAllAsync();
        return courses.Select(course => new GroupDto
        {
            GroupId = course.Id,
            Name = course.Name
        }).ToList();
    }
}
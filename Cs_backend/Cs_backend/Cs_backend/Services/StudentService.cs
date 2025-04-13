using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Services;

public class StudentService(StudentRepository submissionRepository)
{
    public async Task AddOrUpdateStudent(Student student)
    {
        await submissionRepository.AddOrUpdateAsync(student);
    }

    public async Task<List<StudentDto>> GetStudentsByGroupId(int groupId)
    {
        var students = await submissionRepository.GetStudentsByGroupId(groupId);
        return students.Select(student => new StudentDto
        {
            StudentId = student.Id,
            Name = student.Name,
            GroupId = student.GroupId,
            TgId = student.TelegramId,
            CourseId = student.CourseId
        }).ToList();
    }
}
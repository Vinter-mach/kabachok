using Cs_backend.DTO;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services.BaseServices;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Services;

public class StudentService(StudentRepository studentRepository)
{
    public async Task AddOrUpdateStudent(Student student)
    {
        await studentRepository.AddOrUpdateAsync(student);
    }

    public async Task<List<StudentDto>> GetStudentsByGroupId(int groupId)
    {
        return await GetterByFunc.GetByFunc<Student, StudentDto>(studentRepository, x => x.GroupId == groupId);
    }
}
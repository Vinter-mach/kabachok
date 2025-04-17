namespace Cs_backend.Models;

public class Teacher : BaseModel
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; }
}


using System.Numerics;

namespace Cs_backend.Models;

public class Teacher : BaseModel
{
    public BigInteger LoginHash { get; set; }
    public BigInteger PasswordHash { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; }
}


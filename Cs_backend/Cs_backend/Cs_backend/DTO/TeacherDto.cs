using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class TeacherDto : ICastable<Teacher>
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public Teacher Cast()
    {
        return new Teacher()
        {
            Login = Login,
            Name = Name,
            PasswordHash = PasswordHash,
        };
    }
}
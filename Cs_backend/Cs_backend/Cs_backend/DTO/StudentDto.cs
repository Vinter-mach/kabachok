using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class StudentDto : ICastable<Student>
{
    public int StudentId { get; set; }
    public required string Name { get; set; }
    
    public required int CourseId { get; set; }
    public int GroupId { get; set; }
    public int TgId { get; set; }

    public string TgUserName { get; set; }

    public int GetTgIdFromUserName()
    {
        // TODO: Дэн сказал что просто получить этот id, но мне не просто
        return TgUserName.GetHashCode();
    }

    public Student Cast()
    => new() { 
        Id = StudentId,
        Name = Name,
        GroupId = GroupId,
        TelegramId = GetTgIdFromUserName(),
        CourseId = CourseId,
        // Тут нужно вроде добавить TgName или что-то такое, но у нас в базе такого поля нет))
    };
}
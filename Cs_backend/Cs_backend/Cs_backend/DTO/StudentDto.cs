using Cs_backend.Interfaces;
using Cs_backend.Models;

namespace Cs_backend.DTO;

public class StudentDto : ICastable<Student>
{
    public int StudentId { get; set; }
    public required string Name { get; set; }
    public required int CourseId { get; set; }
    public int GroupId { get; set; }
    public required int TgId { get; set; }
    

    public Student Cast()
    => new() { 
        Id = StudentId,
        Name = Name,
        GroupId = GroupId,
        TelegramId = TgId,
        CourseId = CourseId,
        // Тут нужно вроде добавить TgName или что-то такое, но у нас в базе такого поля нет))
    };
}

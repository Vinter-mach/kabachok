namespace Cs_backend.Models;

public class Document : BaseModel
{
    public Guid CarId { get; set; }
    public Guid WorkerId { get; set; }
}
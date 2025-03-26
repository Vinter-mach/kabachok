using Cs_backend.Models;

namespace Cs_backend.Services;

public class RepairService
{
    private BaseRepository<Document> Documents { get; set; }

    public void Work()
    {
        var rand = new Random();
        var carId = Guid.NewGuid();
        var workerId = Guid.NewGuid();

        Documents.Create(new Document {
            CarId = carId,
            WorkerId = workerId,
        });
    }
}
using Cs_backend.Models;
using Cs_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cs_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MainController(RepairService repairService, BaseRepository<Document> document)
    : ControllerBase
{
    private RepairService RepairService { get; set; } = repairService;
    private BaseRepository<Document> Documents { get; set; } = document;

    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(Documents.GetAll());
    }

    [HttpPost]
    public JsonResult Post()
    {
        RepairService.Work();
        return new JsonResult("Work was successfully done");
    }

    [HttpPut]
    public JsonResult Put(Document doc)
    {
        var success = true;
        var document = Documents.Get(doc.Id);
        try
        {
            if (document != null)
            {
                document = Documents.Update(doc);
            }
            else
            {
                success = false;
            }
        }
        catch (Exception)
        {
            success = false;
        }

        return success
            ? new JsonResult($"Update successful {document.Id}")
            : new JsonResult("Update was not successful");
    }

    [HttpDelete]
    public JsonResult Delete(Guid id)
    {
        bool success = true;
        var document = Documents.Get(id);

        try
        {
            if (document != null)
            {
                Documents.Delete(document.Id);
            }
            else
            {
                success = false;
            }
        }
        catch (Exception)
        {
            success = false;
        }

        return success ? new JsonResult("Delete successful") : new JsonResult("Delete was not successful");
    }
}
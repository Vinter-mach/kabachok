using Cs_backend.Database;
using Cs_backend.Models;

public class BaseRepository<TDbModel>(ApplicationContext context)
    where TDbModel : BaseModel
{
    private ApplicationContext Context { get; set; } = context;

    public TDbModel Create(TDbModel model)
    {
        Context.Set<TDbModel>().Add(model);
        Context.SaveChanges();
        return model;
    }

    public void Delete(Guid id)
    {
        var toDelete = Context.Set<TDbModel>().Find(id);
        Context.Set<TDbModel>().Remove(toDelete);
        Context.SaveChanges();
    }

    public List<TDbModel> GetAll()
    {
        return Context.Set<TDbModel>().ToList();
    }

    public TDbModel Update(TDbModel model)
    {
        var toUpdate = Context.Set<TDbModel>().Find(model.Id);
        if (toUpdate != null)
        {
            toUpdate = model;
        }
        Context.Update(toUpdate);
        Context.SaveChanges();
        return toUpdate;
    }

    public TDbModel? Get(Guid id)
    {
        return Context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
    }
}
using DataTrack.DataBase;
using DataTrack.Model.Utils;
using DataTrack.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataTrack.Repositories.Implementation;

public class CrudRepository<T> : ICrudRepository<T> where T : class, IBaseEntity
{
    protected DatabaseContext _context;
    protected DbSet<T> _entities;

    public CrudRepository(DatabaseContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    //TODO: implement semaphore later
    public virtual async Task<IEnumerable<T>> ReadAll()
    {
        IEnumerable<T> data;
        data = _entities.ToList();
        return data;
    }

    public virtual async Task<T> Read(Guid id)
    {
        T data;
        data = _entities.FirstOrDefault(e => e.Id == id);
        return data;
    }

    public virtual async Task<T> Create(T entity)
    {
        _entities.Add(entity);
        _context.SaveChanges();
        await Task.Delay(1);
        return entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        T entityToUpdate;
        
        entityToUpdate = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (entityToUpdate != null)
        {
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            await Task.Delay(1);
        }
        
        return entityToUpdate;
    }

    public virtual async Task<T> Delete(Guid id)
    {
        T entityToDelete;

        entityToDelete=_entities.FirstOrDefault(e => e.Id == id);
        if (entityToDelete != null)
        {
            _context.Remove(entityToDelete);
            _context.SaveChanges();
            await Task.Delay(1);
        }
        
        return entityToDelete;
    }
}
using DataTrack.Config;
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
        await ScadaConfig.dbSemaphore.WaitAsync();
        IEnumerable<T> data;
        try
        {
            data = await _entities.ToListAsync();
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        return data;
    }

    public virtual async Task<T> Read(Guid id)
    {
        await ScadaConfig.dbSemaphore.WaitAsync();
        T data;
        try
        {
            data = await _entities.FirstOrDefaultAsync(e => e.Id == id);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        return data;
    }

    public virtual async Task<T> Create(T entity)
    {
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
            await Task.Delay(1);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        
        return entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        T entityToUpdate;
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            entityToUpdate = await _entities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                await Task.Delay(1);
            }
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }

        return entityToUpdate;
    }

    public virtual async Task<T> Delete(Guid id)
    {
        T entityToDelete;
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            entityToDelete= await _entities.FirstOrDefaultAsync(e => e.Id == id);
            if (entityToDelete != null)
            {
                _context.Remove(entityToDelete);
                await _context.SaveChangesAsync();
                await Task.Delay(1);
            }
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }

        return entityToDelete;
    }
}
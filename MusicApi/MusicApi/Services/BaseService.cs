using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Model;

namespace MusicApi.Services;

public record ServiceResult<TResult>(bool HasError = false, string? Error = null, TResult? Result = default);
public record CountResult(string Action, long Processed);

public interface IBaseService<T>
{
    public Task<ServiceResult<List<T>>> GetAsync(params Expression<Func<T, object>>[] includes);
    public Task<ServiceResult<T>> GetAsync(int id, params Expression<Func<T, object>>[] includes);
    public Task<ServiceResult<CountResult>> InsertAsync(T entity);
    public Task<ServiceResult<CountResult>> DeleteAsync(int id);
    public Task<ServiceResult<CountResult>> UpdateAsync(int id, Action<T> del);
}

/// <summary>
/// Generic service that will handle CRUD operations for Artist, Album and Song entities
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
{
    private readonly DatabaseContext _dbContext;
    public BaseService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResult<List<T>>> GetAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await ExecuteSafelyAsync(async () => await query.ToListAsync());
    }

    public async Task<ServiceResult<T>> GetAsync(int id, params Expression<Func<T, object>>[] includes) =>
        await ExecuteSafelyAsync(async () => await GetByIdAsync(id, includes) ?? throw new Exception("Query yielded no results"));
    
    public async Task<ServiceResult<CountResult>> InsertAsync(T entity)
    {
        return await ExecuteSafelyAsync(async () =>
        {
            await _dbContext.Set<T>().AddAsync(entity);
            var inserted = await _dbContext.SaveChangesAsync();
            return new CountResult("Inserted", inserted);
        });
    }
    
    public async Task<ServiceResult<CountResult>> DeleteAsync(int id)
    {
        return await ExecuteSafelyAsync(async () =>
        {
            var deleted = 0;
            
            var entity = await GetByIdAsync(id);
            if (entity is not null)
            {
                _dbContext.Set<T>().Remove(entity);
                deleted = await _dbContext.SaveChangesAsync();
            }

            return new CountResult("Deleted", deleted);
        });
    }

    /// <summary>
    /// Updates an entity that matches the provided ID
    /// </summary>
    /// <param name="id">Entity ID to update</param>
    /// <param name="del">Delegate to update properties of Entity</param>
    /// <returns></returns>
    public async Task<ServiceResult<CountResult>> UpdateAsync(int id, Action<T> del)
    {

        return await ExecuteSafelyAsync(async () =>
        {
            var updated = 0;
            
            var entityToUpdate = await GetByIdAsync(id);
            if (entityToUpdate is not null)
            {
                del(entityToUpdate);
                updated = await _dbContext.SaveChangesAsync();
            }

            return new CountResult("Updated", updated);
        });
    }

    private async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    // Allows a custom message rather than exception msg
    protected ValueTask<ServiceResult<TResult>> HandleServiceError<TResult>(string message) => new (new ServiceResult<TResult>(true, message));
    private async Task<ServiceResult<TResult>> ExecuteSafelyAsync<TResult>(Func<Task<TResult>> del)
    {
        try
        {
            var result = await del();
            return new ServiceResult<TResult>(Result: result);
        }
        catch (Exception ex)
        {
            return await HandleServiceError<TResult>(ex.Message);
        }
    }
}
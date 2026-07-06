using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Data;
using System.Linq.Expressions;
namespace KnowYourCustomerServiceForBank.Server.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T>
    where T : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<List<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();

    public async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);
}
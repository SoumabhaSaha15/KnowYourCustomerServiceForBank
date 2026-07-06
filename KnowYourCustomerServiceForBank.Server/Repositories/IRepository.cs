using System.Linq.Expressions;
namespace KnowYourCustomerServiceForBank.Server.Repositories;

public interface IRepository<T> where T : class
{
  Task<T?> GetByIdAsync(int id);

  Task<List<T>> GetAllAsync();

  Task AddAsync(T entity);

  void Update(T entity);

  void Delete(T entity);

  Task SaveChangesAsync();

  Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate);
}
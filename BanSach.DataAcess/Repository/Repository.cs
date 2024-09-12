using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebStoreBook.DataAcess.Repository.IRepository;

namespace WebStoreBook.DataAcess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> DbSet;
		public Repository(ApplicationDbContext db)
		{
			_db = db;
			this.DbSet = _db.Set<T>();
		}
		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = DbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(item);
				}
			}
			return query.ToList();
		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			IQueryable<T> query = DbSet;
			if (includeProperties != null)
			{
				foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(item);
				}
			}
			query = query.Where(filter);
			return query.FirstOrDefault();
		}

		public void Remove(T entity)
		{
			DbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			DbSet.RemoveRange(entities);
		}
	}
}

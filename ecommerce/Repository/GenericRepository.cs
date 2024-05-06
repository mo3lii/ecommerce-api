using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repository
{
    public class GenericRepository<TEntity> where TEntity : class,IEntity
	{	
		ecommerceContext _context;
		public GenericRepository(ecommerceContext context) {
			_context = context;
		}
		public List<TEntity> GetAll()
		{
			return _context.Set<TEntity>().ToList();
		}

        public TEntity GetById(int id,params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            foreach (var include in includes)
			{
                query = query.Include(include);
			}
			return query.FirstOrDefault(p=>p.Id==id);

        }
        public void Insert(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Added;
		}
		public void Update(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Deleted;
		}

		public TEntity GetFirstByFilter(Func<TEntity, bool> filter)
		{
			return _context.Set<TEntity>().FirstOrDefault(filter);
		}
	}
}

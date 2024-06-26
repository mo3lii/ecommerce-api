﻿using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repository
{
    public class GenericRepository<TEntity,TID> where TEntity : class,IEntity<TID>
	{	
		ecommerceContext _context;
		public GenericRepository(ecommerceContext context) {
			_context = context;
		}
		public List<TEntity> GetAll(params string[] includes)
		{
			var query = _context.Set<TEntity>().Where(e => e.isDeleted == false&&e.isActive==true).AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
			return query.ToList();
        }
        public List<TEntity> GetAll(Func<TEntity, bool> filter, params string[] includes)
        {
            var query = _context.Set<TEntity>().Where(e => e.isDeleted == false && e.isActive == true).Where(filter).AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }

        public TEntity GetById(TID id,params string[] includes)
        {
            var query = _context.Set<TEntity>().Where(e => e.isDeleted == false && e.isActive == true).AsQueryable();
            foreach (var include in includes)
			{
                query = query.Include(include);
			}
			return query.FirstOrDefault(p=>p.Id.Equals(id));

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
			return _context.Set<TEntity>().Where(e => e.isDeleted == false && e.isActive == true).FirstOrDefault(filter);
		}

        public void softDelete(TEntity entity)
        {
            entity.isDeleted = true;
        }

    }
}

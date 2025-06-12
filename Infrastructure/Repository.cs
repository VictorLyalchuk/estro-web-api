using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Infrastructure.Data;

namespace Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DataBaseContext _context;
        internal DbSet<TEntity> dbSet;
        public Repository(DataBaseContext context)
        {
            _context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            await Task.Run
            (
               () =>
               {
                   if (filter != null)
                   {
                       query = query.Where(filter);
                   }

                   foreach (var includeProperty in includeProperties)
                   {
                       query = query.Include(includeProperty);
                   }
               });
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        public async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }
        public async Task DeleteAsync(TEntity entityToDelete)
        {
            await Task.Run
                (
            () =>
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            });
        }
        public async Task Update(TEntity entityToUpdate)
        {
            var model = _context.Entry(entityToUpdate);
            model.State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            await Task.Run
        (
        () =>
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                dbSet.Attach(entityToUpdate);
            }
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        });
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<TEntity>> GetListBySpec(Ardalis.Specification.ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }
        public async Task<TEntity?> GetItemBySpec(Ardalis.Specification.ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();

        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(dbSet, specification);
        }
        public async Task Attach(TEntity entity)
        {
            await Task.Run
            (
                () =>
                        {
                            _context.Attach(entity);
                        });
        }
    }
}

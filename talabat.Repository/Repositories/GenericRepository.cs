using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.Repository.Data;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Specification;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>) await _context.Products.Include(P=>P.ProductType).Include(P=>P.ProductBrand).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _context.Products.Where(P=>P.Id==id).Include(P => P.ProductType).Include(P => P.ProductBrand).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecifictions<T> spec )
        {
            return await ApplySpecifications(spec).CountAsync();
        }


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifictions<T> spec)
        {
           return await  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec).ToListAsync();

        }

        public async Task<T?> GetWithSpecAsync(ISpecifictions<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications (ISpecifictions<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }


        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);


        public void Delete(T entity) => _context.Set<T>().Remove(entity);
       

        public void Update(T entity)=> _context.Set<T>().Update(entity);
       
    }
}


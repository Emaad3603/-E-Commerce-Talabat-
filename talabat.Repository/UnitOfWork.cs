using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.Repository.Data;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Services.InterFaces;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;

        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();


        public async ValueTask DisposeAsync()=> await _context.DisposeAsync();
      
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);

                _repositories.Add(type, repository);
            }

           return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}

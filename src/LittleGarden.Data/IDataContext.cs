using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LittleGarden.Core.Entities;

namespace LittleGarden.Data
{
    public interface IDataContext<T> where T : IEntity
    {
        Task Save(T entity, Expression<Func<T,bool>> filter);

        Task<T> GetOne(string field, object value);
        Task<IEnumerable<T>> GetAll(PageConfig page);
        Task<bool> Create(T entity, Expression<Func<T, bool>> filter);
        Task<string[]> GetIds(Expression<Func<T, bool>> filter);
    }
}
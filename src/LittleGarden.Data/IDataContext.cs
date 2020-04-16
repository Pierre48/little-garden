using System.Collections.Generic;
using System.Threading.Tasks;
using LittleGarden.Core.Entities;

namespace LittleGarden.Data
{
    public interface IDataContext<T> where T : Entity
    {
        Task Save(T entity);

        Task<T> GetOne(string field, object value);
        Task<IEnumerable<T>> GetAll(PageConfig page);
    }
}
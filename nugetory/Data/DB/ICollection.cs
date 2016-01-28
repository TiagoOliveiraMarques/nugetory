using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using nugetory.Data.Entities;

namespace nugetory.Data.DB
{
    public interface ICollection<T> where T : class, IBaseEntity
    {
        Task<long> Count();

        Task<long> Count(Func<T, bool> query);

        Task<bool> Create(T item);

        //Task<bool> Create(T[] items);

        Task<List<T>> Read();

        Task<List<T>> Read(Func<T, bool> query);

        Task<T> Read(string id);

        Task<bool> Update(T item);

        //Task<bool> Update(T[] items);

        Task<bool> Delete(string id);

        //Task<bool> Delete(string[] ids);

        //Task<long> Delete(Func<T, bool> filter);
    }
}

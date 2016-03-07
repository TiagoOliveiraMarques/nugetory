using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nugetory.Data.Entities;

namespace nugetory.Data.DB
{
    public class JSONMemoryStore<T> : ICollection<T> where T : class, IBaseEntity
    {
        public IDictionary<string, T> EntityCollection { get; set; }

        public JSONMemoryStore()
        {
            EntityCollection = new Dictionary<string, T>();
        }

        public Task<long> Count()
        {
            return Task.FromResult(EntityCollection.LongCount());
        }

        public Task<long> Count(Func<T, bool> query)
        {
            return Task.FromResult(EntityCollection.Values.LongCount(query));
        }

        public Task<bool> Create(T item)
        {
            if (item == null)
                return Task.FromResult(false);

            if (EntityCollection.ContainsKey(item.Id))
                return Task.FromResult(false);

            EntityCollection.Add(item.Id, item);


            return Task.FromResult(true);
        }

        public Task<List<T>> Read()
        {
            return Task.FromResult(EntityCollection.Values.ToList());
        }

        public Task<List<T>> Read(Func<T, bool> query)
        {
            return Task.FromResult(query == null ? new List<T>(0) : EntityCollection.Values.Where(query).ToList());
        }

        public Task<T> Read(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Task.FromResult(default(T));

            if (!EntityCollection.ContainsKey(id))
                return Task.FromResult(default(T));

            return Task.FromResult(EntityCollection[id]);
        }

        public Task<bool> Update(T item)
        {
            if (item == null)
                return Task.FromResult(false);

            if (!EntityCollection.ContainsKey(item.Id))
                return Task.FromResult(false);

            EntityCollection[item.Id] = item;

            return Task.FromResult(true);
        }

        public Task<bool> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Task.FromResult(false);

            if (!EntityCollection.ContainsKey(id))
                return Task.FromResult(false);

            EntityCollection.Remove(id);

            return Task.FromResult(true);
        }
    }
}

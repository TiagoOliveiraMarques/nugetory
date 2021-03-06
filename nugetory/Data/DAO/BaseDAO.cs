﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using nugetory.Data.Entities;

namespace nugetory.Data.DAO
{
    public class BaseDAO<T> where T : class, IBaseEntity
    {
        protected readonly DB.ICollection<T> Entities;

        protected BaseDAO(DB.ICollection<T> entities)
        {
            Entities = entities;
        }

        public virtual Task<long> Count(Func<T, bool> query = null)
        {
            return query == null ? Entities.Count() : Entities.Count(query);
        }

        public virtual Task<bool> Create(T entity)
        {
            return Entities.Create(entity);
        }

        public virtual Task<T> Read(string id)
        {
            return Entities.Read(id);
        }

        public virtual Task<List<T>> Read(Func<T, bool> query = null)
        {
            return query == null ? Entities.Read() : Entities.Read(query);
        }

        public virtual Task<bool> Update(T entity)
        {
            return Entities.Update(entity);
        }

        public virtual Task<bool> Delete(string id)
        {
            return Entities.Delete(id);
        }
    }
}

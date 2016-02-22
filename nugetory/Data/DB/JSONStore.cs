using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using nugetory.Data.Entities;
using Newtonsoft.Json;

namespace nugetory.Data.DB
{
    public class JSONStore<T> : ICollection<T> where T : class, IBaseEntity
    {
        public string Filename { get; set; }

        public JSONStore(string filename)
        {
            Filename = filename;

            string configFileDirectory = Path.GetDirectoryName(Filename);

            if (configFileDirectory == null)
                throw new ArgumentException("Invalid configuration file location");

            if (!Directory.Exists(configFileDirectory))
                Directory.CreateDirectory(configFileDirectory);
        }

        public Task<long> Count()
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(0L);

                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    T[] entities = (T[]) serializer.Deserialize(file, typeof(T[]));

                    return Task.FromResult(entities.LongLength);
                }
            }
            catch (Exception)
            {
                return Task.FromResult(-1L);
            }
        }

        public Task<long> Count(Func<T, bool> query)
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(0L);

                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    T[] entities = (T[]) serializer.Deserialize(file, typeof(T[]));

                    return Task.FromResult(entities.Where(query).LongCount());
                }
            }
            catch (Exception)
            {
                return Task.FromResult(-1L);
            }
        }

        public Task<bool> Create(T item)
        {
            try
            {
                List<T> entities;
                if (System.IO.File.Exists(Filename))
                {
                    using (StreamReader file = System.IO.File.OpenText(Filename))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                    }
                }
                else
                    entities = new List<T>();

                entities.Add(item);

                using (StreamWriter file = System.IO.File.CreateText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, entities.ToArray());
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        //public Task<bool> Create(T[] items)
        //{
        //    try
        //    {
        //        List<T> entities;
        //        if (System.IO.File.Exists(Filename))
        //        {
        //            using (StreamReader file = System.IO.File.OpenText(Filename))
        //            {
        //                JsonSerializer serializer = new JsonSerializer();
        //                entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
        //            }
        //        }
        //        else
        //            entities = new List<T>();

        //        entities.AddRange(items);

        //        using (StreamWriter file = System.IO.File.CreateText(Filename))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();
        //            serializer.Serialize(file, entities.ToArray());
        //        }
        //        return Task.FromResult(true);
        //    }
        //    catch (Exception)
        //    {
        //        return Task.FromResult(false);
        //    }
        //}

        public Task<List<T>> Read()
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(new List<T>(0));

                List<T> entities;
                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                }

                return Task.FromResult(entities);
            }
            catch (Exception)
            {
                return Task.FromResult(new List<T>(0));
            }
        }

        public Task<List<T>> Read(Func<T, bool> query)
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(new List<T>(0));

                List<T> entities;
                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                }

                return Task.FromResult(entities.Where(query).ToList());
            }
            catch (Exception)
            {
                return Task.FromResult(new List<T>(0));
            }
        }

        public Task<T> Read(string id)
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(default(T));

                List<T> entities;
                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                }

                return Task.FromResult(entities.FirstOrDefault(e => e.Id == id));
            }
            catch (Exception)
            {
                return Task.FromResult(default(T));
            }
        }

        public Task<bool> Update(T item)
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(false);

                List<T> entities;
                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                }

                if (entities.RemoveAll(e => e.Id == item.Id) == 1)
                {
                    entities.Add(item);

                    using (StreamWriter file = System.IO.File.CreateText(Filename))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, entities.ToArray());
                    }
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        //public Task<bool> Update(T[] items)
        //{
        //    try
        //    {
        //        if (!System.IO.File.Exists(Filename))
        //            return Task.FromResult(false);

        //        List<T> entities;
        //        using (StreamReader file = System.IO.File.OpenText(Filename))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();
        //            entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
        //        }

        //        if (entities.RemoveAll(e => items.Any(i => i.Id == e.Id)) == items.LongLength)
        //        {
        //            entities.AddRange(items);

        //            using (StreamWriter file = System.IO.File.CreateText(Filename))
        //            {
        //                JsonSerializer serializer = new JsonSerializer();
        //                serializer.Serialize(file, entities.ToArray());
        //            }
        //            return Task.FromResult(true);
        //        }
        //        return Task.FromResult(false);
        //    }
        //    catch (Exception)
        //    {
        //        return Task.FromResult(false);
        //    }
        //}

        public Task<bool> Delete(string id)
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                    return Task.FromResult(true);

                List<T> entities;
                using (StreamReader file = System.IO.File.OpenText(Filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
                }

                if (entities.RemoveAll(e => e.Id == id) == 1)
                {
                    using (StreamWriter file = System.IO.File.CreateText(Filename))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, entities.ToArray());
                    }
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        //public Task<bool> Delete(string[] ids)
        //{
        //    try
        //    {
        //        if (!System.IO.File.Exists(Filename))
        //            return Task.FromResult(true);

        //        List<T> entities;
        //        using (StreamReader file = System.IO.File.OpenText(Filename))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();
        //            entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
        //        }

        //        if (entities.RemoveAll(e => ids.Any(id => e.Id == id)) == ids.LongLength)
        //        {
        //            using (StreamWriter file = System.IO.File.CreateText(Filename))
        //            {
        //                JsonSerializer serializer = new JsonSerializer();
        //                serializer.Serialize(file, entities.ToArray());
        //            }
        //            return Task.FromResult(true);
        //        }
        //        return Task.FromResult(false);
        //    }
        //    catch (Exception)
        //    {
        //        return Task.FromResult(false);
        //    }
        //}

        //public Task<long> Delete(Func<T, bool> filter)
        //{
        //    try
        //    {
        //        if (!System.IO.File.Exists(Filename))
        //            return Task.FromResult(0L);

        //        List<T> entities;
        //        using (StreamReader file = System.IO.File.OpenText(Filename))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();
        //            entities = ((T[]) serializer.Deserialize(file, typeof(T[]))).ToList();
        //        }

        //        List<T> entitiesToRemove = entities.Where(filter).ToList();
        //        int removedEntities = entities.RemoveAll(e => entitiesToRemove.Contains(e));

        //        if (!entitiesToRemove.Any())
        //            return Task.FromResult(-1L);

        //        if (entitiesToRemove.Count() == removedEntities)
        //        {
        //            using (StreamWriter file = System.IO.File.CreateText(Filename))
        //            {
        //                JsonSerializer serializer = new JsonSerializer();
        //                serializer.Serialize(file, entities.ToArray());
        //            }
        //            return Task.FromResult((long) removedEntities);
        //        }
        //        return Task.FromResult(-1L);
        //    }
        //    catch (Exception)
        //    {
        //        return Task.FromResult(-1L);
        //    }
        //}
    }
}

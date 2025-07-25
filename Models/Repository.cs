// 创建文件 Repositories/Repository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Repositories
{
    // 实现泛型仓储，使用泛型约束
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _entities = new List<T>();
        private readonly object _lock = new object();
        private int _nextId = 1;
        
        public void Add(T entity)
        {
            lock (_lock)
            {
                entity.Id = _nextId++;
                _entities.Add(entity);
            }
        }
        
        public bool Remove(int id)
        {
            lock (_lock)
            {
                int index = _entities.FindIndex(e => e.Id == id);
                if (index < 0)
                    return false;
                    
                _entities.RemoveAt(index);
                return true;
            }
        }
        
        public T GetById(int id)
        {
            lock (_lock)
            {
                return _entities.FirstOrDefault(e => e.Id == id);
            }
        }
        
        public IEnumerable<T> GetAll()
        {
            lock (_lock)
            {
                return _entities.ToList(); // 返回副本以避免集合修改异常
            }
        }
        
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _entities.Where(predicate).ToList();
            }
        }
    }
}
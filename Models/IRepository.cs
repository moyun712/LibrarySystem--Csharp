// 创建文件 Repositories/IRepository.cs
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Repositories
{
    // 定义一个接口，表示所有实体都应该有一个ID
    public interface IEntity
    {
        int Id { get; set; }
    }
    
    // 定义一个泛型仓储接口，使用泛型约束要求T必须是IEntity的实现
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        bool Remove(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
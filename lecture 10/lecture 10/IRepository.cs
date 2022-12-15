using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_10
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;
        Task<T> GetById<T>(int id) where T : class;
        Task Create<T>(T entity) where T : class;
        Task Update<T>(T entity) where T : class;
        Task Delete<T>(T entity) where T : class;
    }
}

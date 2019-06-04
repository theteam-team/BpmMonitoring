using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Monitoring.Interfaces
{
    public interface IRepository<T> where T : class
    {
        

        //IEnumerable<T> Find(Func<T, bool> predicate);

        Task<T> GetById(object id);

        Task<List<T>> GetAll();
        Task Create(T entity); 
       
    }
}

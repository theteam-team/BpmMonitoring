

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Text;
using Monitoring.Data;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Monitoring.Interfaces;

namespace Monitoring.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
      
        private readonly DataDbContext _datadbContext;
        

        

       
        public Repository(DataDbContext datadbContext )
        {
           
            _datadbContext = datadbContext;
            
        }
        public async Task Create(T entity)
        {
            //await Task.Run(()=>InitiateConnection());
            _datadbContext.Add(entity);
            _datadbContext.SaveChanges();
        }

        

        
        public async Task<List<T>> GetAll()
        {
            
                //await Task.Run(() => InitiateConnection());
                return _datadbContext.Set<T>().ToList();
          
            
        }


      
        public async Task<T> GetById(object id)
        {
            //await Task.Run(() => InitiateConnection());
            return _datadbContext.Find<T>(id);
        }
       
    }
}

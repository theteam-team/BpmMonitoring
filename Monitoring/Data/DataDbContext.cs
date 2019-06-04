using Monitoring.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Data
{
   
    /// <summary>
    /// this class works as an interface to the database containing the data of the modules
    /// </summary>

    public class DataDbContext : DbContext 
    {
        /// This proberty represents a table in the database to the Modules Entity and can be used to make a CRUD operation on this table 
        //public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
       
        public DbSet<NodeLangWorkflow> NodeLangWorkflow { get; set; }
        /// this  proberty used to set the connection string to the database
        /// 
        public DataDbContext(DbContextOptions<DataDbContext> optionsBuilder) : base(optionsBuilder) { }
        
        

    }
}

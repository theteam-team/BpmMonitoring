using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Data;

using Monitoring.Interfaces;
using Monitoring.Repository;
using Microsoft.AspNetCore.Identity;

namespace Monitoring.Repository
{
    public class NodeLangRepository : Repository<NodeLangWorkflow> , INodeLangRepository
    {
        private readonly DataDbContext _dataDbContext;
       

        public NodeLangRepository(DataDbContext datadbContext) : base(datadbContext)
        {
            _dataDbContext = datadbContext;
           
        }

       
    }
}

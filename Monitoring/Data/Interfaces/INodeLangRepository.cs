using Monitoring.Data;
using Monitoring.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Interfaces
{
    public interface INodeLangRepository : IRepository<NodeLangWorkflow>
    {
        /*Task<List<WorkflowInstance>> GetRunningInstances(string WorkflowId);
        Task<int> GetRunningInstanceCount(string WorkflowId);
        Task AddRunningInstance(string WorkflowId, string InstanceId, string currentNode);
        Task<string> GetCurrentNode(string InstanceId);
        Task UpdateCurrentNode(string InstanceId, string NodeId);*/
        
    }
}

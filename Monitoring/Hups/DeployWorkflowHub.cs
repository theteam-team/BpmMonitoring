using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Interfaces;
using System;
using Monitoring.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Monitoring.Hubs
{

    public class DeployWorkflowHub : Hub
    {
        private readonly DataDbContext _dataDbContext;
        private readonly ILogger<DeployWorkflowHub> _logger;
        private readonly INodeLangRepository _nodeLangRepository;


        public DeployWorkflowHub(ILogger<DeployWorkflowHub> logger ,DataDbContext dataDbContext, INodeLangRepository nodeLangRepository)
        {
            _dataDbContext = dataDbContext;

            _logger = logger;
            _nodeLangRepository = nodeLangRepository;
        }

        
     
        public async Task InitializeDeployList()
        {
            _logger.LogInformation("InitializeDeployList");
            await Clients.Others.SendAsync("InitializeDeployList");
        }
        public async Task updateDeployList(string name, List<string> runningInstances)
        {
            _logger.LogInformation("updateDeployList");
            await Clients.Others.SendAsync("updateDeployList",  name, runningInstances);

        }



        /// Insatances
        public async Task GetRunningWorkFlowInstances(string WorkflowId)
        {                      
            await Clients.Group("Engine").SendAsync("GetRunningWorkFlowInstances", WorkflowId, Context.ConnectionId);           
        }

        public async Task InitializeRuningInstances()
        {

            _logger.LogInformation("InitializeRuningInstances");
            await Clients.Others.SendAsync("InitializeRuningInstances");
         
        }
        
        public async Task AddRunningInstance(string WorkflowId, string InstanceId)
        {                
            await Clients.OthersInGroup(WorkflowId).SendAsync("AddRunningInstance", WorkflowId, InstanceId);
          
            await Clients.OthersInGroup("Deployment").SendAsync("updateNumberOfInstances", WorkflowId);         
        }


     
        public async Task InitializeExecution(string workflowID, string InstanceId)
        {
            
            await Clients.Group("Engine").SendAsync("InitializeExecution", workflowID, InstanceId);
        }
        public async Task UpdateExecution(string workflowID, string InstanceId, List<string> nodeID)
        {
            _logger.LogInformation("UpdateExecution");
            await Clients.Group(workflowID).SendAsync("UpdateExecution", workflowID, InstanceId, nodeID);
        }




        public async Task AddToGroup(string groupName)
        {
            _logger.LogInformation(groupName);
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }




    }
}
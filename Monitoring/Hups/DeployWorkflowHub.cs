using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Interfaces;
using System;
using Monitoring.Data;

namespace Monitoring.Hubs
{

    public class DeployWorkflowHub : Hub
    {
        private readonly DataDbContext _dataDbContext;
   
        private readonly INodeLangRepository _nodeLangRepository;


        public DeployWorkflowHub(DataDbContext dataDbContext, INodeLangRepository nodeLangRepository)
        {
            _dataDbContext = dataDbContext;
            
            _nodeLangRepository = nodeLangRepository;
        }

        /// Deployment
        /*public async Task GetCurrentDeployed()
        {

           
            List<NodeLangWorkflow> nodeLangWorkflows = await _nodeLangRepository.GetDeployedWorkflowsAsync();
           

            if (nodeLangWorkflows != null)
            {

                await Clients.Caller.SendAsync("InitializeDeployList", nodeLangWorkflows);
               
            }
        }*/
        public async Task InitializeDeployList()
        {
            await Clients.Others.SendAsync("InitializeDeployList");
        }
        public async Task updateDeployList(string name, string runningInstances)
        {
            await Clients.Others.SendAsync("updateDeployList",  name, runningInstances);
            
        }



        /// Insatances
        public async Task GetRunningWorkFlowInstances(string WorkflowId)
        {                      
            await Clients.Group("Engine").SendAsync("GetRunningWorkFlowInstances", WorkflowId, Context.ConnectionId);           
        }

        public async Task InitializeRuningInstances()
        {
         
            await Clients.Others.SendAsync("InitializeRuningInstances");
            

        }
        
        public async Task AddRunningInstance(string WorkflowId, string InstanceId)
        {                
            await Clients.OthersInGroup(WorkflowId).SendAsync("AddRunningInstance", WorkflowId, InstanceId);
          
            await Clients.OthersInGroup("Deployment").SendAsync("updateNumberOfInstances", WorkflowId);         
        }


        /// Executing 

        /*public async Task GetExecutedNodes(string workflowId, string instanceId)
        {
            await Clients.Group("Engine").SendAsync("GetExecutedNodes", workflowId, instanceId);

        }*/
        public async Task InitializeExecution(string workflowID, string InstanceId)
        {
            
            Console.WriteLine(workflowID);
            await Clients.Group("Engine").SendAsync("InitializeExecution", workflowID, InstanceId);
        }
        public async Task UpdateExecution(string workflowID, string InstanceId, List<string> nodeID)
        {
            
            await Clients.Group(workflowID).SendAsync("UpdateExecution", workflowID, InstanceId, nodeID);
        }




        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }




    }
}
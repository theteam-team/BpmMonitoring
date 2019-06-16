﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Data;

using Monitoring.Interfaces;
using Monitoring.Repository;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Json;
namespace Monitoring.Repository
{
    public class NodeLangRepository : Repository<NodeLangWorkflow> , INodeLangRepository
    {
        private readonly DataDbContext _dataDbContext;
        //private readonly HttpClient _httpClient;


        public NodeLangRepository(DataDbContext datadbContext) : base(datadbContext)
        {
            _dataDbContext = datadbContext;

        }
        public async Task<List<NodeLangWorkflow>> GetDeployedWorkflowsAsync()
        {
            using (var _httpClient = new HttpClient()) {
                try
                {
                    var response = await _httpClient.GetAsync("http://localhost:8090/engine/api/workflows");
                    List<NodeLangWorkflow> workflows =  new List<NodeLangWorkflow>();
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        //Console.Write(data);
                        workflows = JsonConvert.DeserializeObject<List<NodeLangWorkflow>>(data);
                        
                    }
                    return workflows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

            
        }

        public async Task<List<WorkFlowInstance>> GetRunningInstances(string workflowName)
        {
            using (var _httpClient = new HttpClient())
            {
                try
                {
              

                    var response = await _httpClient.GetAsync("http://localhost:8090/engine/api/workflowinstance?name=" + workflowName);
                    NodeLangWorkflow workflows= new NodeLangWorkflow();
                    List<WorkFlowInstance> workflowInstances = new List<WorkFlowInstance>();
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        workflows = JsonConvert.DeserializeObject<NodeLangWorkflow>(data);
                        workflowInstances = workflows.instances;
                        Console.WriteLine(workflowInstances);
                    }
                    return workflowInstances;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public  async Task<string> GetWorkflowXml(string Id)
        {
            using (var _httpClient = new HttpClient())
            {
                try
                {
                    var response = await _httpClient.GetAsync("http://localhost:8090/engine/api/workflowbody?name=" + Id);

                    string workflow = null;
                    if (response.IsSuccessStatusCode)
                    {
                        workflow = await response.Content.ReadAsStringAsync();
                        //Console.Write(workflow);

                    }
                    return workflow;
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Monitoring.Data;
using Monitoring.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monitoring.Interfaces;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Monitoring.Controllers
{
 
    public class NodeLangController : Controller
    {
       
        private readonly IHubContext<DeployWorkflowHub> _hubcontext;
        private readonly DeployWorkflowHub _monitoringHub;
        private INodeLangRepository _nodeLangRepository;
        private IConfiguration _config;
        private ILogger<NodeLangController> _logger;
        private readonly HttpClient _httpClient;

        public NodeLangController(INodeLangRepository nodeLangRepository, IHubContext<DeployWorkflowHub> hubcontext, ILogger<NodeLangController> logger, IConfiguration config)
        {
          
            _config = config;
            _logger = logger;
       
        

            _hubcontext = hubcontext;
            _nodeLangRepository = nodeLangRepository;
        }

        [HttpGet("")]
        public IActionResult Monitoring_Deployer()
        {
            return View();
        }
        [HttpGet("monitoring/{id}")]
        public IActionResult Monitoring_Workflow([FromRoute]string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpGet("GetWorkFlowXml/{id}")]
        public async Task<ActionResult<string>> GetWorkFlowXml([FromRoute]string id)
        {
            //Guid guid = Guid.Parse(id);
           
            var workFlow = await _nodeLangRepository.GetWorkflowXml(id);
            string WorkFlowStr = workFlow;
            return WorkFlowStr;
            
        }

        [HttpGet("GetDeployedWorkFlows")]
        public async Task<ActionResult<List<NodeLangWorkflow>>> GetDeployedWorkFlows()
        {

            var workFlow = await _nodeLangRepository.GetDeployedWorkflowsAsync();
            if (workFlow != null)
            {
                return Ok(workFlow);
            }
            return Ok(new List<NodeLangWorkflow>());
        }

        [HttpGet("GetRunningInstances/{WorkflowName}")]
        public async Task<ActionResult<List<WorkFlowInstance>>> GetRunningInstances([FromRoute]string WorkflowName)
        {

            var workFlow = await _nodeLangRepository.GetRunningInstances(WorkflowName);
            if (workFlow != null)
            {
                return Ok(workFlow);
            }
            return Ok(new List<string>());
        }
        [HttpGet("GetProcessesInstance/{instanceId}")]
        public async Task<ActionResult<List<BpmProcess>>> GetProcessesInstance([FromRoute]string instanceId)
        {

            var workFlow = await _nodeLangRepository.GetProcessesInstance(instanceId);
            if (workFlow != null)
            {
                return Ok(workFlow);
            }
            return Ok(new List<BpmProcess>());
        }

        


        [HttpPost("UploadWorkFlow")]
        public async Task<IActionResult> UploadWorkFlow(BpmWorkFlow bpmWorkFlow)
        {

            Console.WriteLine(bpmWorkFlow.workflowName);

            _logger.LogInformation("Bpm Invoker Service Invoking Workflow " + bpmWorkFlow.workflowName);
            using (var client = new HttpClient())
            {
                try
                {
                    string url = _config["BpmEngine:Address"] + "/engine/api/workflow?name=";
                    _logger.LogInformation("Sending to  " + url);
                    await client.PostAsJsonAsync(url + bpmWorkFlow.workflowName, bpmWorkFlow.workFlowParam);

                }
                catch (Exception)
                {

                    throw;
                }
            }
            //try
            //{
            //    bool isCopied = false;
            //    if (file.Length > 0)
            //    {
            //        string fileName = file.FileName;                   
            //        string extension = Path.GetExtension(fileName);
            //        if (extension == ".xml")
            //        {
            //            string workFlowStr = null;
            //            string WorkFlowName = null;
            //            string workFlowId = null;
            //            using (Stream stream = file.OpenReadStream())
            //            {
            //                BinaryReader br = new BinaryReader(stream);
            //                byte[] fileBytes = br.ReadBytes((int)stream.Length);
            //                workFlowStr = Encoding.ASCII.GetString(fileBytes);

            //                XmlDocument xmlDoc = new XmlDocument();
            //                stream.Position = 0;
            //                xmlDoc.Load(stream);
            //                var element = xmlDoc.GetElementsByTagName("nodes")[0];
            //                if (element == null)
            //                {
            //                    throw new Exception("This xml file is not supported");
            //                }
            //                WorkFlowName = element.Attributes["name"].Value;
            //                //workFlowId = element.Attributes["id"].Value;
            //                Console.WriteLine(WorkFlowName);
            //            }
            //            string filePath = Path.GetFullPath(
            //                Path.Combine(Directory.GetCurrentDirectory(),
            //                                            "wwwroot/WorkFlows"));
            //            using (var fileStream = new FileStream(
            //                Path.Combine(filePath, fileName),
            //                                FileMode.Create))
            //            {
            //                await file.CopyToAsync(fileStream);
            //                isCopied = true;


            //            }
            //            if (isCopied)
            //            {

            //                string idstr = workFlowId;

            //                Console.WriteLine(idstr);

            //                await _nodeLangRepository.Create(new NodeLangWorkflow
            //                {
            //                    //Id = workFlowId,
            //                    Name = WorkFlowName,
            //                    //WorkFlow = workFlowStr,
            //                    RuningInstances = 0
            //                }
            //                );
            //                await _hubcontext.Clients.All.SendAsync("updateDeployList", idstr, WorkFlowName, workFlowStr);
            //                return Ok();
            //            }
            //        }
            //        else
            //        {
            //            throw new Exception("File must be  .xml");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return BadRequest("error");

            }
        }

}
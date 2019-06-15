using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monitoring.Hubs;
using Monitoring.Interfaces;

namespace Monitoring.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class NodeLangApiController : ControllerBase
    {
        private IHubContext<DeployWorkflowHub> _hubcontext;
        private INodeLangRepository _nodeLangRepository;

        public NodeLangApiController(INodeLangRepository nodeLangRepository, IHubContext<DeployWorkflowHub> hubcontext)
        {

            _hubcontext = hubcontext;
            _nodeLangRepository = nodeLangRepository;
        }
        
    }
}
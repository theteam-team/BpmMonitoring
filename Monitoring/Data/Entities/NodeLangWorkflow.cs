using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Data
{
    
    public class NodeLangWorkflow
    {

       
        public string Name { get; set; }

        
        public int RuningInstances { get; set; }
        [JsonProperty("instances")]
        public List<WorkFlowInstance> instances { get; set; }

        //public List<WorkflowInstance> WorkflowInstances{ get; set; }
       
      
    }
}

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

        [JsonIgnore]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string WorkFlow { get; set;}

        public long RuningInstances { get; set; }

        //public List<WorkflowInstance> WorkflowInstances{ get; set; }
       
       
    }
}

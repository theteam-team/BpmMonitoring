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
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public string WorkFlow { get; set;}

        public long RuningInstances { get; set; }

        //public List<WorkflowInstance> WorkflowInstances{ get; set; }
       
       
    }
}

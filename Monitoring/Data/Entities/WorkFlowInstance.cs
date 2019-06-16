using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Data
{
   

    public class WorkFlowInstance
    {
        [JsonProperty("instanceID")]
        public string Id { get; set; }
        [JsonIgnore]
        public string WorkFlowName { get; set; }
    }
}

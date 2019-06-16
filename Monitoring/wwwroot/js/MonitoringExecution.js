let Runninginstances = {};
let Nodes = [];
let WorkFlowid = $('workflow').attr('id');
let currentInstance;
let connection = new signalR.HubConnectionBuilder().withUrl("/DeployWorkflowHub").build();
connection.start().then(function ()
{
    connection.invoke("AddToGroup", WorkFlowid);
    $.getJSON("/GetRunningInstances/" + WorkFlowid,
        function (runingInstances) {
            console.log(runingInstances);

            for (var i = 0; i < runingInstances.length; i++) {
                CreateInstanceRecord(runingInstances[i].instanceID);
            }
        });
});

connection.on("UpdateExecution", function (workflowID, InstanceId, nodeID)
{
    console.log("UpdateExecution");
    UpdateExecution(workflowID, InstanceId, nodeID)
});

connection.on("InitializeRuningInstances", function () {
    $.getJSON("/GetRunningInstances/" + WorkFlowid,
        function (runingInstances) {
            if (runingInstances) {
                console.log(runingInstances);
                for (var i = 0; i < runingInstances.length; i++) {
                    CreateInstanceRecord(runingInstances[i].instanceID);
                }
            }
        });
    // console.log("InitializeRuningInstances ");
    //for (var i = 0; i < instances.length; ++i) {
    //    if (!Runninginstances[instances[i]])
    //        Runninginstances[instances[i]] = { "CurrentNode": [] };
    //    connection.invoke("InitializeExecution", WorkFlowid, instances[i]);
    //    CreateInstanceRecord(instances[i])
    //}
});
connection.on("AddRunningInstance", function (WorkflowId, InstanceId) {
    CreateInstanceRecord(InstanceId);
    /*if (!Runninginstances[InstanceId])
    {
        Runninginstances[InstanceId] = { "CurrentNode": [] }
        Runninginstances[InstanceId] = currentNode;
        console.log(currentNode);
        CreateInstanceRecord(InstanceId);
    }*/
});


function CreateInstanceRecord(runingInstanceId)
{
    
    var list = $("#runningInstances");   
    list.append("<li><input type = 'button'  id =" + runingInstanceId + " value=" + runingInstanceId + "></li>")
    $("#" + runingInstanceId).on('click',function () { changeInstance(runingInstanceId); });
}

function changeInstance(runingInstanceId)
{
    resetdraw();
    currentInstance = runingInstanceId;
    console.log(Runninginstances);
    connection.invoke("InitializeExecution", WorkFlowid, runingInstanceId);
    
}

function UpdateExecution(workflowID,  InstanceId,  nodes)
{
    console.log(nodeID);
    if (InstanceId == currentInstance) {
        Nodes = nodes;
        //Runninginstances[InstanceId] = { "CurrentNode": [] }
        //Runninginstances[InstanceId].CurrentNode = nodeID;
        if (InstanceId == currentInstance) {
            for (var i = 0; i < Nodes.length; ++i) {
                var NodeId = Nodes[i];
                var name = $("#" + NodeId).attr('nodeName');
                $("#" + NodeId).attr('href', "/img/nodes/" + name + "_chosen.png")
            }
        }
    }
   
}
let Runninginstances = {};
let WorkFlowid = $('workflow').attr('id');
let currentInstance;
let connection = new signalR.HubConnectionBuilder().withUrl("/DeployWorkflowHub").build();
connection.start().then(function ()
{
    connection.invoke("AddToGroup", WorkFlowid);
    connection.invoke("GetRunningWorkFlowInstances", WorkFlowid)
});

connection.on("UpdateExecution", function (workflowID, InstanceId, nodeID)
{
    console.log("UpdateExecution");
    UpdateExecution(workflowID, InstanceId, nodeID)
});

connection.on("InitializeRuningInstances", function (workflowId, instances) {
     console.log("InitializeRuningInstances ");
    for (var i = 0; i < instances.length; ++i) {
        if (!Runninginstances[instances[i]])
            Runninginstances[instances[i]] = { "CurrentNode": [] };
        connection.invoke("InitializeExecution", WorkFlowid, instances[i]);
        CreateInstanceRecord(instances[i])
    }
});
connection.on("AddRunningInstance", function (WorkflowId, InstanceId, currentNode) {
    if (!Runninginstances[InstanceId])
    {
        Runninginstances[InstanceId] = { "CurrentNode": [] }
        Runninginstances[InstanceId] = currentNode;
        console.log(currentNode);
        CreateInstanceRecord(InstanceId);
    }
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
    for (var i = 0; i < Runninginstances[currentInstance].CurrentNode.length; ++i)
    {
        var NodeId = Runninginstances[runingInstanceId].CurrentNode[i];
        var name = $("#" + NodeId).attr('nodeName');
        $("#" + NodeId).attr('href', "/img/nodes/" + name + "_chosen.png")
    }
}
function UpdateExecution( workflowID,  InstanceId,  nodeID)
{
    console.log(nodeID);
    if (!Runninginstances[InstanceId])
        Runninginstances[InstanceId] = { "CurrentNode": [] }
    Runninginstances[InstanceId].CurrentNode = nodeID;
    if (InstanceId == currentInstance)
    {
        changeInstance(InstanceId);
    }
   
}
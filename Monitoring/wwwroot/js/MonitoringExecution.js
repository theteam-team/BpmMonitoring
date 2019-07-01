let Runninginstances = {};
let WorkFlowid = $('workflow').attr('id');
let currentInstance;
let connection = new signalR.HubConnectionBuilder().withUrl("/DeployWorkflowHub").build();
connection.start().then(function ()
{
    connection.invoke("AddToGroup", WorkFlowid);
    $.getJSON("/GetRunningInstances/" + WorkFlowid,
        function (runingInstances) {
            for (var i = 0; i < runingInstances.length; i++) {
                CreateInstanceRecord(runingInstances[i].instanceID);
            }
        });
});

connection.on("UpdateExecution", function (workflowID, InstanceId, nodeID) {
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
   
});


function CreateInstanceRecord(runingInstanceId)
{
    var instance = document.getElementById(runingInstanceId);   
    if (!instance) {
       
        var list = $("#runningInstances");
        list.append("<li><input type = 'button'  id =" + runingInstanceId + " value=" + runingInstanceId + "></li>")
        $("#" + runingInstanceId).on('click', function () { changeInstance(runingInstanceId); });
    }
    else {
        console.log("Found");
    }
}

function changeInstance(runingInstanceId)
{
    Runninginstances[runingInstanceId] = { "nodes": [] }
    var nodes = [];
    $.getJSON("/GetProcessesInstance/" + runingInstanceId,
        function (runingInstances) {
            if (runingInstances) {
                for (var i = 0; i < runingInstances.length; i++) {
                    nodes[i] = runingInstances[i].processID;
                    console.log(nodes[i])
                }
            }
             Runninginstances[runingInstanceId].nodes = nodes;
            initializeExecution(runingInstanceId);
        });

}

function UpdateExecution(workflowID,  InstanceId,  nodes)
{
    CreateInstanceRecord(InstanceId);
    Runninginstances[InstanceId] = { "nodes": [] }
    Runninginstances[InstanceId].nodes = nodes;
    if (InstanceId == currentInstance) {
      
        initializeExecution(InstanceId);
    }
    
   
}

function initializeExecution(runingInstanceId)
{
    resetdraw();
    currentInstance = runingInstanceId;
    var nodes = Runninginstances[runingInstanceId].nodes;
    console.log(Runninginstances[runingInstanceId].nodes);
    for (var i = 0; i < nodes.length; ++i) {
        var NodeId = nodes[i];
        var name = $("#" + NodeId).attr('nodeName');
        $("#" + NodeId).attr('href', "/img/nodes/" + name + "_chosen.png")
    }

}
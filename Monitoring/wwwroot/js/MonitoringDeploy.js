

let connection = new signalR.HubConnectionBuilder().withUrl("/DeployWorkflowHub").build();
connection.start().then(function ()
{
    connection.invoke("AddToGroup", "Deployment")
    connection.invoke("GetCurrentDeployed")
});

connection.on("updateDeployList", function (id, name, workFlowStr) {
    CreateWorkflowRecord(id, name, 0)
       
});
connection.on("InitializeDeployList", function (deployedWorkFlows) {
    console.log(deployedWorkFlows);
    for (var i = 0; i < deployedWorkFlows.length; ++i)
    {
        var workflow = deployedWorkFlows[i];
        CreateWorkflowRecord(workflow.id, workflow.name, workflow.runingInstances)     
    }
   
});

connection.on("updateNumberOfInstances", function (workflowId, runningInstances)
{
    updateNumberOfInstances(workflowId, runningInstances);
    
});

function updateNumberOfInstances(workflowId ,runningInstances)
{
    //console.log("runningInstances: " + runningInstances);
    $('#' + workflowId).children('li')[1].innerHTML = "Running Instances: " + runningInstances;
}

function CreateWorkflowRecord(id, name, runingInstances)
{
    var list = $("#workFlows");
    list.append("<ul id =" + id + " style = 'list-style:none' ></ul>")
    var newList = $("#" + id)
    newList.append("<li style = display:inline; ><a href = /monitoring/" + id + ">" + name + "</a></li>")
    newList.append("<li style = 'display:inline;margin:10px'; >Running Instances : " + runingInstances + "</li>")
}

$('#uploadWorkflow').on('submit', function ()
{
    
    var formData = new FormData($("#uploadWorkflow")[0]);
    var that = $(this),
        url = that.attr('action'),
        type = that.attr('method'),
        encrypt = that.attr('enctype')
        data = {},
    that.find('[name]').each(function (index, value) {
        var that = $(this);
        var name = that.attr('name');
        var value = that.val();
        data[name] = value
    });
    console.log(data);
    $.ajax(
        {
            url: url,
            type: type,
            processData: false,
            contentType: false,
            data: formData,
            success: function (response)
            {
                console.log(response);
            }
        });
    return false;
})
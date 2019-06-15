

let connection = new signalR.HubConnectionBuilder().withUrl("/DeployWorkflowHub").build();
connection.start().then(function ()
{
    connection.invoke("AddToGroup", "Deployment");
    $.getJSON("/GetDeployedWorkFlows",
        function (workflows) {
            console.log(workflows);
            for (var i = 0; i < workflows.length; i++) {
                CreateWorkflowRecord(workflows[i].name, workflows[i].runingInstances)
            }
        });
});

connection.on("updateDeployList", function (id, name, workFlowStr) {
    CreateWorkflowRecord(name, 0)
       
});
connection.on("InitializeDeployList", function () {
    
    $.getJSON("/GetDeployedWorkFlows",
          function (workflows) {
            console.log(workflows);
            for (var i = 0; i < workflows.length; i++) {
                CreateWorkflowRecord(workflows[i].name, workflows[i].runingInstances)
            }
        });
   
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

function CreateWorkflowRecord(name, runingInstances)
{
    var list = $("#workFlows");
    list.append("<ul id =" + name + " style = 'list-style:none' ></ul>")
    var newList = $("#" + name)
    newList.append("<li style = display:inline; ><a href = /monitoring/" + name + ">" + name + "</a></li>")
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
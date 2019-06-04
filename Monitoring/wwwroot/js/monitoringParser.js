let WorkFlowObject =
{
    "Nodes": {},
    "Variables": {},
    "Position": {}
    
};
let loaded = false;
let xmlns = "http://www.w3.org/2000/svg";
let xlinkns = "http://www.w3.org/1999/xlink";
let WorkFlow = document.getElementById("WorkFlow");
parse();
function parse() {
    if (!loaded) {
        var id = $('workflow').attr('id');
        var xml = new XMLHttpRequest();
        xml.open('Get', "/GetWorkFlow/"+id, true);
        xml.send();
        
        xml.onload = function () {
            var xmlData = xml.responseText;
            var xmlWorkFlow = new DOMParser().parseFromString(xmlData, "text/xml");
            var startNode = xmlWorkFlow.getElementsByTagName("start")[0];
            var currentNode = startNode;
            //console.log(currentNode.getAttribute('nId') + " " + currentNode.tagName);

            drawImage(currentNode, xmlWorkFlow);

            Parse(xmlWorkFlow, currentNode);

            loaded = true;

        }
    }
}
function Parse(xmlWorkFlow, currentNode)
{
   
    var nextNodeID = currentNode.getElementsByTagName("nextNode");
    for (var i = 0; i < nextNodeID.length; ++i)
    {
        var nextNode = xmlWorkFlow.getElementsByTagName("nodes")[0].querySelectorAll("[nId='" + nextNodeID[i].innerHTML + "']")[0];     
        drawImage(nextNode, xmlWorkFlow);
        drawLine(currentNode, nextNode);
        //console.log(nextNodeID[i].innerHTML + " " + currentNode.tagName);
        if (currentNode.tagName != "end")
            Parse(xmlWorkFlow, nextNode);
    }
    
}
function drawLine(currentNode, nextNode)
{
    var currentNodeId = currentNode.getAttribute("nId");
    var nextNodeId = nextNode.getAttribute("nId");
    var x1 = +WorkFlowObject.Nodes[currentNodeId].x + +WorkFlowObject.Nodes[currentNodeId].width / 2;
    var y1 = WorkFlowObject.Nodes[currentNodeId].y;
    var x2 = +WorkFlowObject.Nodes[nextNodeId].x - +WorkFlowObject.Nodes[currentNodeId].width / 2;
    var y2 = WorkFlowObject.Nodes[nextNodeId].y;
     
    var xDis = x2 - x1;
    var yDis = y2 - y1;
    var path = document.createElementNS(xmlns, 'path');
    var path_s = "M" + x1 + " " + y1 + " h " + (8 * xDis) / 10 + " v " + yDis + " L " + x2 + " " + y2;
    path.setAttribute('id', currentNodeId + nextNodeId);
    path.setAttribute('stroke', "black");
    path.setAttribute('stroke-width', "2");
    path.setAttribute('stroke-linecap', "round");
    path.setAttribute('d', path_s);
    path.setAttribute('fill', "none")
    path.setAttribute('marker-end', "url(#arrowhead)");

    WorkFlow.appendChild(path);
}

function drawImage(currentNode, xmlDoc) {
    var imgSrc = '/img/nodes/' + currentNode.tagName + '.png';
    var node = WorkFlowObject.Nodes;
    var img = document.createElementNS(xmlns, 'image');
    var currentNodeId = currentNode.getAttribute("nId");
    img.setAttribute('height', '100');
    img.setAttribute('width', '100');
    img.setAttribute ('id', currentNodeId);
    img.setAttribute ('nodeName', currentNode.tagName);
    img.setAttributeNS(xlinkns, "href", imgSrc);
    var Type = currentNode.getAttribute("type");
    node[currentNodeId] = {
        "img": undefined, "name": currentNode.tagName
        , "type": Type, "x": 0, "y": 0, "width": 0
    };
    getPosition(xmlDoc, currentNode);
    var posX = node[currentNodeId].x;
    var posY = node[currentNodeId].y;
    img.setAttribute("x", node[currentNodeId].x);
    img.setAttribute("y", node[currentNodeId].y);
    node[currentNodeId].img = img;
    
    WorkFlow.appendChild(img);
    editImagePosition(currentNodeId)
    
}

function editImagePosition(currentNodeId)
{
    var x = WorkFlowObject.Nodes[currentNodeId].x;
    var y = WorkFlowObject.Nodes[currentNodeId].y;
    //console.log(x + " " + y);

    var img = document.getElementById(currentNodeId);
    var rect = img.getBoundingClientRect();
    //console.log(rect);
    
    var cx = +x + rect.width * 0.5;    // find center of first image
    var cy = +y + rect.width * 0.5;
    WorkFlowObject.Nodes[currentNodeId].x = cx;
    WorkFlowObject.Nodes[currentNodeId].y = cy;
    WorkFlowObject.Nodes[currentNodeId].width = rect.width;
    //console.log(cx + " " + cy);
}
function getPosition(xmlDoc , currentNode)
{
    var currentNodeId = currentNode.getAttribute("nId");
    var NodePosition = xmlDoc.getElementsByTagName("positions")[0]
        .querySelectorAll("[nId='" + currentNodeId + "']")[0];
    var x = NodePosition.getElementsByTagName("x")[0].innerHTML;
    var y = NodePosition.getElementsByTagName("y")[0].innerHTML;
    WorkFlowObject.Nodes[currentNodeId].x = x;
    WorkFlowObject.Nodes[currentNodeId].y = y;
        
}

function resetdraw()
{

    var nodes = Object.keys(WorkFlowObject.Nodes);
    for (var i = 0; i < nodes.length; ++i)
    {
        console.log("reseting");
        var name = $("#" + nodes[i]).attr('nodeName');
        $("#" + nodes[i]).attr('href', "/img/nodes/" + name + ".png")
    }
}

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public struct GridCoord { public int col, row; }

public interface IGridNode
{
    GridCoord Coord { get; set; }
}

public class VNNode : Node, IGridNode
{
    public GridCoord Coord { get; set; }
    public override void SetPosition(Rect newPos)
    {
        float cellW = 120f;
        float cellH = 120f;

        int col = Mathf.RoundToInt(newPos.x / cellW);
        int row = Mathf.RoundToInt(newPos.y / cellH);

        base.SetPosition(new Rect(
            col * cellW,
            row * cellH,
            cellW,
            cellH
        ));
        Coord = new GridCoord { col = col, row = row };
    }
    // … 커맨드 데이터 필드 …
}
public class StartNode : VNNode
{
    public StartNode()
    {
        title = "Start Node"; // 노드 제목
        style.width = 120;
        style.height = 120;
        var label = new Label("The story will \nbegin at this node");
        mainContainer.Add(label);
        RefreshExpandedState();
        RefreshPorts();
    }
    public sealed override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
    { 
        var port = base.InstantiatePort(orientation, direction, capacity, type);
        return port;
    }
}

public class EndNode : VNNode
{
    public EndNode()
    {
        title = "End Node"; // 노드 제목
        style.width = 120;
        style.height = 120;
        var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(object));
        outputPort.portName = "Input";
        outputContainer.Add(outputPort);
        var label = new Label("End ");
        mainContainer.Add(label);
        RefreshExpandedState();
        RefreshPorts();
    }
    public sealed override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
    { 
        var port = base.InstantiatePort(orientation, direction, capacity, type);
        return port;
    }
}